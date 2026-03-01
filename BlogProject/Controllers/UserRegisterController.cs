using BlogProject.Dtos;
using BlogProject.Models;
using Entities.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

[Route("[controller]")]
public class UserRegisterController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public UserRegisterController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [Route("[action]")]
    public IActionResult Register()
    {
        return View();
    }
    
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (registerDto.ConfirmPassword != registerDto.Password)
        {ModelState.AddModelError("ConfirmPassword", "Şifreler uyuşmuyor.");
            return View(registerDto);
        }
        
        if (!ModelState.IsValid)
            return View(registerDto);
        
        var appUser = new AppUser()
        {
            NameSurname = registerDto.NameSurname,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };
        
        var result = await _userManager.CreateAsync(appUser, registerDto.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("Login", "UserLogin");
        }
        
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
        
        return View();
    }
}