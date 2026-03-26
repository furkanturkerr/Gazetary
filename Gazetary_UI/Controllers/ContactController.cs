using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

public class ContactController : Controller
{
    
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [Route("iletisim")]
    [HttpGet]
    public IActionResult Iletisim() => View();

    [Route("iletisim")]
    [HttpPost]
    public IActionResult Iletisim(Contact contact)
    {
        _contactService.Insert(contact);
        TempData["Basarili"] = "Mesajınız alındı, teşekkürler!";
        return RedirectToAction("Iletisim");
    }
}