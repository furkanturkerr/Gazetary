using Business.Abstract;
using Business.ValidationsRules;
using Entities.Concrate;
using FluentValidation.Results;
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
        CategoryValidation categoryValidation = new CategoryValidation();
        ValidationResult result = categoryValidation.Validate(category);
        if (result.IsValid)
        {
            _categoryService.Insert(category);
            return RedirectToAction("CategoryList");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
        return View(category);
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
        CategoryValidation categoryValidation = new CategoryValidation();
        ValidationResult result = categoryValidation.Validate(category);
        if (result.IsValid)
        {
            _categoryService.Update(category);
            return RedirectToAction("CategoryList");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
        return View(category);
    }

    public IActionResult DeleteCategory(int id)
    {
        var value = _categoryService.GetById(id);
        _categoryService.Delete(value);
        return RedirectToAction("CategoryList");
    }
    
    public IActionResult ChangeStatus(int id)
    {
        _categoryService.TChangeStatus(id);
        return RedirectToAction("CategoryList");
    }
}