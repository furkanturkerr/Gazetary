using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

[Route("")]
public class CategoryController : Controller
{
    private readonly IBlogPostService _blogPostService;
    private readonly ICategoryService _categoryService;

    public CategoryController(IBlogPostService blogPostService, ICategoryService categoryService)
    {
        _blogPostService = blogPostService;
        _categoryService = categoryService;
    }

    // /oyun
    // /yazilim
    [HttpGet("{categorySlug}")]
    public IActionResult Index(string categorySlug)
    {
        // 1️⃣ KATEGORİ VAR MI?
        var category = _categoryService
            .GetAll()
            .FirstOrDefault(x => x.CategorySlug == categorySlug);

        if (category == null)
            return NotFound();

        // 2️⃣ BU KATEGORİYE AİT YAZILAR
        var posts = _blogPostService
            .TGetCategoryWithBlogPosts()
            .Where(x =>
                x.Category != null &&
                x.Category.CategorySlug == categorySlug)
            .ToList();

        // 3️⃣ KATEGORİ ADI (yazı olmasa bile)
        ViewBag.CategoryName = category.CategoryName;

        return View(posts);
    }


}