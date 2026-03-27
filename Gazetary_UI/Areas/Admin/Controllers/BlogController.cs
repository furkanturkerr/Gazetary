using BlogProject.Areas.Admin.Models;
using BlogProject.Models;
using Business.Abstract;
using Business.ValidationsRules;
using Entities.Concrate;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogProject.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("Admin/[controller]/[action]/{id?}")]
public class BlogController : Controller
{
    private readonly IBlogPostService _blogPostService;
    private readonly ICategoryService _categoryService;
    private readonly IImageService _imageService;

    public BlogController(IBlogPostService blogPostService, ICategoryService categoryService, IImageService imageService)
    {
        _blogPostService = blogPostService;
        _categoryService = categoryService;
        _imageService = imageService;
    }

    public IActionResult Index(string status = "all", int page = 1, string q = "", int category = 0)
    {
        var allPosts = _blogPostService.TGetCategoryWithBlogPosts()
            .OrderByDescending(x => x.CreatedDate)
            .ToList();

        var publishedCount = allPosts.Count(x => x.Status);
        var draftCount     = allPosts.Count(x => !x.Status);

        if (!string.IsNullOrWhiteSpace(q))
            allPosts = allPosts
                .Where(x => x.Title.Contains(q, StringComparison.OrdinalIgnoreCase)
                          || x.Category.CategoryName.Contains(q, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (status == "published")
            allPosts = allPosts.Where(x => x.Status).ToList();
        else if (status == "draft")
            allPosts = allPosts.Where(x => !x.Status).ToList();

        if (category > 0)
            allPosts = allPosts.Where(x => x.CategoryId == category).ToList();

        const int pageSize = 15;
        var totalCount     = allPosts.Count;
        var paged          = allPosts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var vm = new AdminBlogListViewModel
        {
            Posts          = paged,
            Categories     = _categoryService.GetAll().OrderBy(x => x.CategoryName).ToList(),
            StatusFilter   = status,
            SearchQuery    = q,
            CategoryFilter = category,
            CurrentPage    = page,
            PageSize       = pageSize,
            TotalCount     = totalCount,
            PublishedCount = publishedCount,
            DraftCount     = draftCount
        };

        return View(vm);
    }

    [HttpGet]
    public IActionResult CreateBlog()
    {
        ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName");
        ViewBag.Images = new SelectList(
            _imageService.GetAll(),
            "ImageUrl",   
            "ImageUrl"   
        );
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateBlog(BlogPost blogPost)
    {
        BlogValidation blogValidation = new BlogValidation();
        ValidationResult result = blogValidation.Validate(blogPost);

        if (result.IsValid)
        {
            blogPost.Status      = true;
            blogPost.CreatedDate = DateTime.Now;
            _blogPostService.Insert(blogPost);
            return RedirectToAction("Index", new { area = "Admin" });
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

        ViewBag.Images = new SelectList(
            _imageService.GetAll(),
            "ImageUrl",   
            "ImageUrl"   
        );
        ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName");
        return View(blogPost);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var value = _blogPostService.GetById(id);
        _blogPostService.Delete(value);
        return RedirectToAction("Index", new { area = "Admin" });
    }

    [HttpGet]
    public IActionResult EditBlog(int id)
    {
        ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName");
        var value = _blogPostService.GetById(id);
        ViewBag.Images = new SelectList(
            _imageService.GetAll(),
            "ImageUrl",   
            "ImageUrl"   
        );
        return View(value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditBlog(BlogPost model)
    {
        var validator = new BlogValidation();
        var result = validator.Validate(model);

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName");
            ViewBag.Images = new SelectList(_imageService.GetAll(), "ImageUrl", "ImageUrl");
            return View(model);
        }

        var existingBlog = _blogPostService.GetById(model.BlogPostId);
        if (existingBlog == null)
            return NotFound();

        existingBlog.Title = model.Title;
        existingBlog.Description = model.Description;
        existingBlog.Content = model.Content;
        existingBlog.ImageUrl = model.ImageUrl;
        existingBlog.ImageDescription = model.ImageDescription;
        existingBlog.Slug = model.Slug;
        existingBlog.CategoryId = model.CategoryId;
        existingBlog.MetaTitle = model.MetaTitle;
        existingBlog.MetaDescription = model.MetaDescription;
        existingBlog.MetaKeywords = model.MetaKeywords;
        
        _blogPostService.Update(existingBlog);

        return RedirectToAction("Index", new { area = "Admin" });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult ChangeStatus(int id)
    {
        _blogPostService.ChangeStatus(id);
        return RedirectToAction("Index", new { area = "Admin" });
    }
}