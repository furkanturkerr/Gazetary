using BlogProject.Dtos;
using Entities.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

[Route("[controller]")]
public class UserLoginController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;

    public UserLoginController(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [Route("[action]")]
    public IActionResult Login()
    {
        return View();
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return View(loginDto);

        var result = await _signInManager.PasswordSignInAsync(
            loginDto.Email,
            loginDto.Password,
            false,
            false
        );

        if (result.Succeeded)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(loginDto.Email);

            return RedirectToAction("Profile", "Profile", new { id = user.Id });
        }

        ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı.");
        return View(loginDto);
    }
    
    [HttpGet]
    public async Task<IActionResult> UserLogout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}