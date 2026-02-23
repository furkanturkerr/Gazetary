using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;

[Area("Admin")]
public class CommentController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}