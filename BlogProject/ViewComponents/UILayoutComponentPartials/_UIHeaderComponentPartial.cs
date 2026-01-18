using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.UILayoutComponentPartials;

public class _UIHeaderComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}