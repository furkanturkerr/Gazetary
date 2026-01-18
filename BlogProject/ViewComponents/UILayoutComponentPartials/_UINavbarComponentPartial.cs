using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.UILayoutComponentPartials;

public class _UINavbarComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}