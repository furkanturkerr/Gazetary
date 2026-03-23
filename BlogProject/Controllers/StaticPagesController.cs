using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

public class StaticPagesController : Controller
{
    [Route("hakkimizda")]
    public IActionResult Hakkimizda() => View();

    [Route("reklam")]
    public IActionResult Reklam() => View();

    [Route("gizlilik")]
    public IActionResult Gizlilik() => View();

    [Route("cerez-politikasi")]
    public IActionResult CerezPolitikasi() => View();

    [Route("kullanim-sartlari")]
    public IActionResult KullanimSartlari() => View();

    [Route("duzeltme-politikasi")]
    public IActionResult DuzeltmePolitikasi() => View();
}