using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

public class DefaultController : Controller
{
    // GET
    public IActionResult Anasayfa()
    {
        return View();
    }
}