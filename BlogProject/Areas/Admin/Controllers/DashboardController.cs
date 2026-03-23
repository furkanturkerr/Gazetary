using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")][Area("Admin")]
public class DashboardController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}