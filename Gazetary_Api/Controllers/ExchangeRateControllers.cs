using Microsoft.AspNetCore.Mvc;

namespace Gazetary_Api.Controllers;

public class ExchangeRateControllers : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}