using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

[Route("[controller]")]
public class UserLoginController : Controller
{
    [Route("[action]")]
    public IActionResult Login()
    {
        return View();
    }
}