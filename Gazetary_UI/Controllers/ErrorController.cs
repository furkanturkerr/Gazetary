using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

public class ErrorController : Controller
{
    [Route("Error/404")]
    public IActionResult NotFoundPage()
    {
        return View("~/Views/Shared/NotFound.cshtml");
    }
}