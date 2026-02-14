using Business.Abstract;
using Business.ValidationsRules;
using Entities.Concrate;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogProject.Areas.Admin.Controllers;

[Area("Admin")]
public class BlogController : Controller
{
    private readonly IBlogPostService _blogPostService;
    private readonly ICategoryService _categoryService;

    public BlogController(IBlogPostService blogPostService, ICategoryService categoryService)
    {
        _blogPostService = blogPostService;
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        var allPosts = _blogPostService.TGetCategoryWithBlogPosts().OrderByDescending(x=>x.CreatedDate).ToList();
        return View(allPosts);
    }
    
    [HttpGet]
    public IActionResult CreateBlog()
    {
        ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName");
        return View();
    }

    [HttpPost]
    public IActionResult CreateBlog(BlogPost blogPost)
    {
        BlogValidation blogValidation = new BlogValidation();
        ValidationResult resultvalidation = blogValidation.Validate(blogPost);
        if (resultvalidation.IsValid)
        {
            blogPost.Status = true;
            blogPost.CreatedDate = DateTime.Now;
            _blogPostService.Insert(blogPost);
            return RedirectToAction("Index", "Blog", new { area = "Admin" });
        }
        else
        {
            foreach (var error in resultvalidation.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
        return View(blogPost);
    }

    public IActionResult Delete(int id)
    {
        var value = _blogPostService.GetById(id);
        _blogPostService.Delete(value);
        return RedirectToAction("Index", "Blog");
    }

    public IActionResult EditBlog(int id)
    {
        ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName");
        var value = _blogPostService.GetById(id);
        return View(value);
    }

    [HttpPost]
    public IActionResult EditBlog(BlogPost blogPost)
    {
        BlogValidation blogValidation = new BlogValidation();
        ValidationResult resultvalidation = blogValidation.Validate(blogPost);
        if (resultvalidation.IsValid)
        {
            _blogPostService.Update(blogPost);
            return RedirectToAction("Index", "Blog", new { area = "Admin" });
        }
        else
        {
            foreach (var error in resultvalidation.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
        return View(blogPost);
        
    }

    public IActionResult ChangeStatus(int id)
    {
        _blogPostService.ChangeStatus(id);
        return RedirectToAction("Index", "Blog", new { area = "Admin" });
    }
}