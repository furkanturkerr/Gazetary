using BlogProject.Areas.Admin.Models;
using Entities.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")][Authorize]
public class RoleController : Controller
{
    private readonly RoleManager<AppRole> _roleManager;

    public RoleController(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }


    // GET
    public IActionResult Index()
    {
        var values = _roleManager.Roles.ToList();
        return View(values);
    }

    [HttpGet]
    public IActionResult CreateRole()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRole(RoleViewModel roleViewModel)
    {
        if (ModelState.IsValid)
        {
            AppRole role = new AppRole()
            {
                Name = roleViewModel.Name
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Role");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }
        return View();
    }
}