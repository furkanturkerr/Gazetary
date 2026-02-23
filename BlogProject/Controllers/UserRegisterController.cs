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
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        var appUser = new AppUser()
        {
            NameSurname = registerModel.NameSurname,
            Email = registerModel.Email,
        };
        var result = await _userManager.CreateAsync(appUser, registerModel.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("Login", "UserLogin");
        }
        return View();
    }
}