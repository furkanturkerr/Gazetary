using Entities.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.UILayoutComponentPartials;

public class _UIHeaderComponentPartial : ViewComponent
{
    private readonly UserManager<AppUser> _userManager;

    public _UIHeaderComponentPartial(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user);
        }
        return View();
    }
}