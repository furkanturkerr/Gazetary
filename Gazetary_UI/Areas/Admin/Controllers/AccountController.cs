using BlogProject.Areas.Admin.Models;
using Entities.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("Admin")]
public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    [Route("panel-9xk2-admin")]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("panel-9xk2-admin")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ViewBag.Error = "E-posta veya şifre hatalı.";
            return View(model);
        }

        if (!await _userManager.IsInRoleAsync(user, "Admin"))
        {
            ViewBag.Error = "Bu alana erişim yetkiniz yok.";
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);

        if (result.Succeeded)
            return Redirect("/Admin/Blog/Index");
        if (result.IsLockedOut)
            ViewBag.Error = "Hesabınız geçici olarak kilitlendi.";
        else
            ViewBag.Error = "E-posta veya şifre hatalı.";

        return View(model);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateUser()
    {
        return View();
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(RegisterViewModel registerViewModel)
    {
        var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
        if (user != null)
        {
            ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
            return View(registerViewModel);
        }
        
        var appUser = new AppUser
        {
            NameSurname    = registerViewModel.NameSurname,
            Email          = registerViewModel.Email,
            UserName       = registerViewModel.Email,
            EmailConfirmed = true
        };
        
        var result = await _userManager.CreateAsync(appUser, registerViewModel.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(appUser, "Admin");
            return RedirectToAction("Users", "Account");
        }
        
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public async Task<IActionResult> Users()
    {
        var allUsers = _userManager.Users.ToList();
 
        var admins = new List<UserViewModel>();
        var users  = new List<UserViewModel>();
 
        foreach (var user in allUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var vm = new UserViewModel
            {
                Id             = user.Id,
                NameSurname    = user.NameSurname,
                Email          = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Role           = roles.FirstOrDefault() ?? "-"
            };
 
            if (roles.Contains("Admin"))
                admins.Add(vm);
            else
                users.Add(vm);
        }
 
        var model = new UserListViewModel
        {
            Admins = admins,
            Users  = users
        };
 
        return View(model);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public async Task<IActionResult> UserLogout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
        return RedirectToAction("Users", "Account");
    }
}