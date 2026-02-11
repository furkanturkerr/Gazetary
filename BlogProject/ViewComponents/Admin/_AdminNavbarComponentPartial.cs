using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Admin;

public class _AdminNavbarComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}