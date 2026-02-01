using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Admin;

public class _AdminHeaderComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}