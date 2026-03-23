using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")][Area("Admin")]
public class ContactController : Controller
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    public IActionResult Index()
    {
        var contacts = _contactService.GetAll()
            .OrderByDescending(x => x.CreatedDate)
            .ToList();

        return View(contacts);
    }

    public IActionResult Detail(int id)
    {
        var contact = _contactService.GetById(id);

        if (contact == null)
            return NotFound();

        if (!contact.IsRead)
        {
            contact.IsRead = true;
            _contactService.Update(contact);
        }

        return View(contact);
    }

    public IActionResult MarkAsRead(int id)
    {
        var contact = _contactService.GetById(id);
        if (contact != null)
        {
            contact.IsRead = true;
            _contactService.Update(contact);
        }
        return RedirectToAction("Detail", new { id });
    }

    public IActionResult MarkAsUnread(int id)
    {
        var contact = _contactService.GetById(id);
        if (contact != null)
        {
            contact.IsRead = false;
            _contactService.Update(contact);
        }
        return RedirectToAction("Detail", new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var contact = _contactService.GetById(id);
        if (contact != null)
            _contactService.Delete(contact);

        return RedirectToAction("Index");
    }
}