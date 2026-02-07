using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;

public class DashboardController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}