using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Admin;

public class _AdminScriptComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}