using BlogProject.Dtos;
using Entities.Concrate;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Microsoft.AspNetCore.RateLimiting;

namespace BlogProject.Controllers;

[Route("[controller]")]
public class UserRegisterController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public UserRegisterController(UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _userManager   = userManager;
        _configuration = configuration;
    }

    [Route("[action]")]
    public IActionResult Register()
    {
        return View();
    }

    [Route("[action]")]
    [HttpPost]
    [EnableRateLimiting("register-limit")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (registerDto.ConfirmPassword != registerDto.Password)
        {
            ModelState.AddModelError("ConfirmPassword", "Şifreler uyuşmuyor.");
            return View(registerDto);
        }

        if (!ModelState.IsValid)
            return View(registerDto);

        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("", "Bu e-posta adresi zaten kullanılıyor.");
            return View(registerDto);
        }

        Random rnd = new();
        int verificationCode = rnd.Next(100000, 1000000);

        var appUser = new AppUser
        {
            NameSurname = registerDto.NameSurname,
            Email       = registerDto.Email,
            UserName    = registerDto.Email,
            MailCode    = verificationCode,
            RegisterDate = DateTime.Now
        };

        var result = await _userManager.CreateAsync(appUser, registerDto.Password);

        if (result.Succeeded)
        {
            try
            {
                await _userManager.AddToRoleAsync(appUser, "User");
                await SendVerificationEmail(registerDto.Email, verificationCode);

                TempData["Email"]          = registerDto.Email;
                TempData["SuccessMessage"] = "Kayıt başarılı! E-posta adresinize doğrulama kodu gönderildi.";

                return RedirectToAction("EmailConfirmation");
            }
            catch
            {
                await _userManager.DeleteAsync(appUser);
                ModelState.AddModelError("", "E-posta gönderilemedi. Lütfen tekrar deneyin.");
                return View(registerDto);
            }
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(registerDto);
    }

    [Route("[action]")]
    [HttpGet]
    public IActionResult EmailConfirmation()
    {
        if (TempData["Email"] == null)
            return RedirectToAction("Register");

        TempData.Keep("Email");

        return View();
    }

    [Route("[action]")]
    [HttpPost]
    [EnableRateLimiting("verify-limit")]
    public async Task<IActionResult> EmailConfirmation(EmailConfirmationDto emailConfirmationDto)
    {
        var email = TempData["Email"]?.ToString();

        if (string.IsNullOrEmpty(email))
            return RedirectToAction("Register");

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            ModelState.AddModelError("", "Kullanıcı bulunamadı.");
            return View(emailConfirmationDto);
        }
        
        if ((DateTime.Now - user.RegisterDate).TotalMinutes > 10)
        {
            await _userManager.DeleteAsync(user);
            TempData["ErrorMessage"] = "Doğrulama süresi doldu. Lütfen tekrar kayıt olun.";
            return RedirectToAction("Register");
        }


        if (user.MailCode != emailConfirmationDto.Code)
        {
            TempData.Keep("Email");
            ModelState.AddModelError("Code", "Doğrulama kodu hatalı. Lütfen tekrar deneyin.");
            return View(emailConfirmationDto);
        }

        user.EmailConfirmed = true;
        user.MailCode       = 0;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            ModelState.AddModelError("", "Doğrulama sırasında bir hata oluştu.");
            return View(emailConfirmationDto);
        }

        TempData["VerifiedMessage"] = "E-posta adresiniz başarıyla doğrulandı. Giriş yapabilirsiniz.";
        return RedirectToAction("Login", "UserLogin");
    }

    [Route("[action]")]
    [HttpPost]
    [EnableRateLimiting("verify-limit")]
    public async Task<IActionResult> ResendCode()
    {
        var email = TempData["Email"]?.ToString();

        if (string.IsNullOrEmpty(email))
            return RedirectToAction("Register");

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return RedirectToAction("Register");

        Random rnd = new();
        int newCode  = rnd.Next(100000, 1000000);
        user.MailCode = newCode;

        await _userManager.UpdateAsync(user);

        try
        {
            await SendVerificationEmail(email, newCode);
            TempData["Email"]          = email;
            TempData["SuccessMessage"] = "Yeni doğrulama kodu e-posta adresinize gönderildi.";
        }
        catch
        {
            TempData["Email"]        = email;
            TempData["ErrorMessage"] = "E-posta gönderilemedi. Lütfen tekrar deneyin.";
        }

        return RedirectToAction("EmailConfirmation");
    }

    private async Task SendVerificationEmail(string email, int code)
    {
        var mimeMessage = new MimeMessage();

        var senderEmail = _configuration["EmailSettings:SenderEmail"];
        var senderName  = _configuration["EmailSettings:SenderName"];
        var smtpServer  = _configuration["EmailSettings:SmtpServer"];
        var smtpPort    = int.Parse(_configuration["EmailSettings:SmtpPort"]!);
        var smtpPassword= _configuration["EmailSettings:SmtpPassword"];

        mimeMessage.From.Add(new MailboxAddress(senderName, senderEmail));
        mimeMessage.To.Add(new MailboxAddress("", email));
        mimeMessage.Subject = "Gazetary.com - E-posta Doğrulama";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"""
                <div style="font-family:Arial,sans-serif;max-width:480px;margin:0 auto;padding:32px;border:1px solid #e5e7eb">
                    <h2 style="font-size:22px;font-weight:700;margin-bottom:8px">Gazetary</h2>
                    <p style="color:#666;margin-bottom:24px">E-posta adresinizi doğrulamak için aşağıdaki kodu kullanın.</p>
                    <div style="background:#f3f4f6;padding:24px;text-align:center;letter-spacing:12px;font-size:32px;font-weight:700;color:#111">
                        {code}
                    </div>
                    <p style="color:#999;font-size:12px;margin-top:24px">Bu kodu kimseyle paylaşmayınız. Kod 10 dakika geçerlidir.</p>
                </div>
            """,
            TextBody = $"Gazetary.com doğrulama kodunuz: {code}\n\nBu kodu kimseyle paylaşmayınız."
        };

        mimeMessage.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpServer, smtpPort, false);
        await client.AuthenticateAsync(senderEmail, smtpPassword);
        await client.SendAsync(mimeMessage);
        await client.DisconnectAsync(true);
    }
}