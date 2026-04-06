using BlogProject.Dtos;
using Dtos;
using Entities.Concrate;
using MailKit.Net.Smtp;
using MailKit.Security;
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
    private readonly ILogger<UserRegisterController> _logger;

    public UserRegisterController(UserManager<AppUser> userManager, IConfiguration configuration, ILogger<UserRegisterController> logger)
    {
        _userManager   = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    // ─────────────────────────────────────────────
    //  KAYIT
    // ─────────────────────────────────────────────

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
            NameSurname  = registerDto.NameSurname,
            Email        = registerDto.Email,
            UserName     = registerDto.Email,
            MailCode     = verificationCode,
            RegisterDate = DateTime.Now
        };

        var result = await _userManager.CreateAsync(appUser, registerDto.Password);

        if (result.Succeeded)
        {
            try
            {
                await _userManager.AddToRoleAsync(appUser, "User");
                _logger.LogInformation("Doğrulama e-postası gönderiliyor: {Email}", registerDto.Email);
                await SendVerificationEmail(registerDto.Email, verificationCode);
                _logger.LogInformation("Doğrulama e-postası başarıyla gönderildi: {Email}", registerDto.Email);

                TempData["Email"]          = registerDto.Email;
                TempData["SuccessMessage"] = "Kayıt başarılı! E-posta adresinize doğrulama kodu gönderildi.";

                return RedirectToAction("EmailConfirmation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Doğrulama e-postası gönderilemedi: {Email}", registerDto.Email);
                await _userManager.DeleteAsync(appUser);
                ModelState.AddModelError("", "E-posta gönderilemedi. Lütfen tekrar deneyin.");
                return View(registerDto);
            }
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(registerDto);
    }

    // ─────────────────────────────────────────────
    //  E-POSTA DOĞRULAMA
    // ─────────────────────────────────────────────

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
        int newCode   = rnd.Next(100000, 1000000);
        user.MailCode = newCode;

        await _userManager.UpdateAsync(user);

        try
        {
            _logger.LogInformation("Yeni doğrulama kodu gönderiliyor: {Email}", email);
            await SendVerificationEmail(email, newCode);
            _logger.LogInformation("Yeni doğrulama kodu başarıyla gönderildi: {Email}", email);
            TempData["Email"]          = email;
            TempData["SuccessMessage"] = "Yeni doğrulama kodu e-posta adresinize gönderildi.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Yeni doğrulama kodu gönderilemedi: {Email}", email);
            TempData["Email"]        = email;
            TempData["ErrorMessage"] = "E-posta gönderilemedi. Lütfen tekrar deneyin.";
        }

        return RedirectToAction("EmailConfirmation");
    }

    // ─────────────────────────────────────────────
    //  ŞİFRE SIFIRLAMA — Adım 1: E-posta girişi
    // ─────────────────────────────────────────────

    [Route("[action]")]
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [Route("[action]")]
    [HttpPost]
    [EnableRateLimiting("register-limit")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        if (!ModelState.IsValid)
            return View(forgotPasswordDto);

        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

        // Güvenlik: kullanıcı bulunamasa bile aynı mesajı göster
        if (user == null || !user.EmailConfirmed)
        {
            TempData["SuccessMessage"] = "Eğer bu e-posta adresine ait bir hesap varsa, sıfırlama kodu gönderildi.";
            return RedirectToAction("ResetPasswordConfirmation");
        }

        Random rnd = new();
        int resetCode       = rnd.Next(100000, 1000000);
        user.MailCode       = resetCode;
        user.PasswordResetRequestedAt = DateTime.Now;

        await _userManager.UpdateAsync(user);

        try
        {
            _logger.LogInformation("Şifre sıfırlama e-postası gönderiliyor: {Email}", forgotPasswordDto.Email);
            await SendPasswordResetEmail(forgotPasswordDto.Email, resetCode);
            _logger.LogInformation("Şifre sıfırlama e-postası başarıyla gönderildi: {Email}", forgotPasswordDto.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Şifre sıfırlama e-postası gönderilemedi: {Email}", forgotPasswordDto.Email);
            ModelState.AddModelError("", "E-posta gönderilemedi. Lütfen tekrar deneyin.");
            return View(forgotPasswordDto);
        }

        TempData["Email"]          = forgotPasswordDto.Email;
        TempData["SuccessMessage"] = "Eğer bu e-posta adresine ait bir hesap varsa, sıfırlama kodu gönderildi.";
        return RedirectToAction("ResetPasswordConfirmation");
    }

    // ─────────────────────────────────────────────
    //  ŞİFRE SIFIRLAMA — Adım 2: Kod doğrulama
    // ─────────────────────────────────────────────

    [Route("[action]")]
    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        TempData.Keep("Email");
        return View();
    }

    [Route("[action]")]
    [HttpPost]
    [EnableRateLimiting("verify-limit")]
    public async Task<IActionResult> ResetPasswordConfirmation(ResetPasswordConfirmationDto dto)
    {
        var email = TempData["Email"]?.ToString();

        if (string.IsNullOrEmpty(email))
            return RedirectToAction("ForgotPassword");

        if (!ModelState.IsValid)
        {
            TempData.Keep("Email");
            return View(dto);
        }

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
            return RedirectToAction("ForgotPassword");
        }

        if (user.PasswordResetRequestedAt == null ||
            (DateTime.Now - user.PasswordResetRequestedAt.Value).TotalMinutes > 10)
        {
            user.MailCode = 0;
            await _userManager.UpdateAsync(user);
            TempData["ErrorMessage"] = "Kodun süresi doldu. Lütfen tekrar şifre sıfırlama isteği oluşturun.";
            return RedirectToAction("ForgotPassword");
        }

        if (user.MailCode != dto.Code)
        {
            TempData.Keep("Email");
            ModelState.AddModelError("Code", "Doğrulama kodu hatalı. Lütfen tekrar deneyin.");
            return View(dto);
        }

        // Kod doğru → bir sonraki adım için token üret
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        TempData["Email"]             = email;
        TempData["ResetToken"]        = token;
        TempData["SuccessMessage"]    = "Kod doğrulandı. Yeni şifrenizi belirleyin.";

        // Kodu temizle
        user.MailCode = 0;
        await _userManager.UpdateAsync(user);

        return RedirectToAction("ResetPassword");
    }

    [Route("[action]")]
    [HttpPost]
    [EnableRateLimiting("verify-limit")]
    public async Task<IActionResult> ResendPasswordResetCode()
    {
        var email = TempData["Email"]?.ToString();

        if (string.IsNullOrEmpty(email))
            return RedirectToAction("ForgotPassword");

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || !user.EmailConfirmed)
        {
            TempData["Email"] = email;
            return RedirectToAction("ResetPasswordConfirmation");
        }

        Random rnd = new();
        int newCode = rnd.Next(100000, 1000000);
        user.MailCode = newCode;
        user.PasswordResetRequestedAt = DateTime.Now;

        await _userManager.UpdateAsync(user);

        try
        {
            _logger.LogInformation("Yeni şifre sıfırlama kodu gönderiliyor: {Email}", email);
            await SendPasswordResetEmail(email, newCode);
            _logger.LogInformation("Yeni şifre sıfırlama kodu başarıyla gönderildi: {Email}", email);
            TempData["Email"] = email;
            TempData["SuccessMessage"] = "Yeni şifre sıfırlama kodu e-posta adresinize gönderildi.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Yeni şifre sıfırlama kodu gönderilemedi: {Email}", email);
            TempData["Email"] = email;
            TempData["ErrorMessage"] = "E-posta gönderilemedi. Lütfen tekrar deneyin.";
        }

        return RedirectToAction("ResetPasswordConfirmation");
    }

    // ─────────────────────────────────────────────
    //  ŞİFRE SIFIRLAMA — Adım 3: Yeni şifre belirleme
    // ─────────────────────────────────────────────

    [Route("[action]")]
    [HttpGet]
    public IActionResult ResetPassword()
    {
        if (TempData["ResetToken"] == null || TempData["Email"] == null)
            return RedirectToAction("ForgotPassword");

        TempData.Keep("Email");
        TempData.Keep("ResetToken");

        return View();
    }

    [Route("[action]")]
    [HttpPost]
    [EnableRateLimiting("register-limit")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var email = TempData["Email"]?.ToString();
        var token = TempData["ResetToken"]?.ToString();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            return RedirectToAction("ForgotPassword");

        if (!ModelState.IsValid)
        {
            TempData.Keep("Email");
            TempData.Keep("ResetToken");
            return View(dto);
        }

        if (dto.NewPassword != dto.ConfirmNewPassword)
        {
            TempData.Keep("Email");
            TempData.Keep("ResetToken");
            ModelState.AddModelError("ConfirmNewPassword", "Şifreler uyuşmuyor.");
            return View(dto);
        }

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
            return RedirectToAction("ForgotPassword");
        }

        var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

        if (result.Succeeded)
        {
            // İsteğe bağlı: aktif oturumları geçersiz kıl
            await _userManager.UpdateSecurityStampAsync(user);

            // Sıfırlama zamanını temizle
            user.PasswordResetRequestedAt = null;
            await _userManager.UpdateAsync(user);

            TempData["VerifiedMessage"] = "Şifreniz başarıyla sıfırlandı. Yeni şifrenizle giriş yapabilirsiniz.";
            return RedirectToAction("Login", "UserLogin");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        TempData.Keep("Email");
        TempData.Keep("ResetToken");
        return View(dto);
    }

    // ─────────────────────────────────────────────
    //  YARDIMCI METODLAR
    // ─────────────────────────────────────────────

    private async Task SendVerificationEmail(string email, int code)
    {
        var mimeMessage = new MimeMessage();

        var senderEmail  = _configuration["EmailSettings:SenderEmail"];
        var senderName   = _configuration["EmailSettings:SenderName"];
        var smtpServer   = _configuration["EmailSettings:SmtpServer"];
        var smtpPort     = int.Parse(_configuration["EmailSettings:SmtpPort"]!);
        var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

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
        await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(senderEmail, smtpPassword);
        await client.SendAsync(mimeMessage);
        await client.DisconnectAsync(true);
    }

    private async Task SendPasswordResetEmail(string email, int code)
    {
        var mimeMessage = new MimeMessage();

        var senderEmail  = _configuration["EmailSettings:SenderEmail"];
        var senderName   = _configuration["EmailSettings:SenderName"];
        var smtpServer   = _configuration["EmailSettings:SmtpServer"];
        var smtpPort     = int.Parse(_configuration["EmailSettings:SmtpPort"]!);
        var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

        mimeMessage.From.Add(new MailboxAddress(senderName, senderEmail));
        mimeMessage.To.Add(new MailboxAddress("", email));
        mimeMessage.Subject = "Gazetary.com - Şifre Sıfırlama";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"""
                <div style="font-family:Arial,sans-serif;max-width:480px;margin:0 auto;padding:32px;border:1px solid #e5e7eb">
                    <h2 style="font-size:22px;font-weight:700;margin-bottom:8px">Gazetary</h2>
                    <p style="color:#666;margin-bottom:24px">Şifrenizi sıfırlamak için aşağıdaki kodu kullanın.</p>
                    <div style="background:#f3f4f6;padding:24px;text-align:center;letter-spacing:12px;font-size:32px;font-weight:700;color:#111">
                        {code}
                    </div>
                    <p style="color:#999;font-size:12px;margin-top:24px">Bu kodu kimseyle paylaşmayınız. Kod 10 dakika geçerlidir.</p>
                    <p style="color:#999;font-size:12px;">Bu isteği siz yapmadıysanız bu e-postayı görmezden gelebilirsiniz.</p>
                </div>
            """,
            TextBody = $"Gazetary.com şifre sıfırlama kodunuz: {code}\n\nBu kodu kimseyle paylaşmayınız. Bu isteği siz yapmadıysanız bu mesajı görmezden gelebilirsiniz."
        };

        mimeMessage.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(senderEmail, smtpPassword);
        await client.SendAsync(mimeMessage);
        await client.DisconnectAsync(true);
    }
}
