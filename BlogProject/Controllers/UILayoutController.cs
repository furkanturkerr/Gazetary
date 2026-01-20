using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

public class UILayoutController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Home()
    {
        return View();
    }
}