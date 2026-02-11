using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Admin;

public class _AdminFooterComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}