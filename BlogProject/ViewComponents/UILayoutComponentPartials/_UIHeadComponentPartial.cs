using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.UILayoutComponentPartials;

public class _UIHeadComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}