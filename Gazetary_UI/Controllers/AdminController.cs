using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

public class AdminController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}