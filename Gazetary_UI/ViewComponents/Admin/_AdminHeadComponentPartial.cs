using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Admin;

public class _AdminHeadComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}