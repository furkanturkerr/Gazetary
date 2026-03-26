using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.UILayoutComponentPartials;

public class _UIFooterComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}