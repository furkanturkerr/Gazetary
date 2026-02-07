using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET
    public IActionResult CategoryList()
    {
        var values = _categoryService.GetAll();
        return View(values);
    }

    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        _categoryService.Insert(category);
        return RedirectToAction("CategoryList");
    }

    [HttpGet]
    public IActionResult EditCategory(int id)
    {
        var category = _categoryService.GetById(id);
        return View(category);
    }

    [HttpPost]
    public IActionResult EditCategory(Category category)
    {
        _categoryService.Update(category);
        return RedirectToAction("CategoryList");
    }

    public IActionResult DeleteCategory(int id)
    {
        var value = _categoryService.GetById(id);
        _categoryService.Delete(value);
        return RedirectToAction("CategoryList");
    }
}