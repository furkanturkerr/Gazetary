using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

[Route("")]
public class CategoryController : Controller
{
    private readonly IBlogPostService _blogPostService;
    private readonly ISeoService _seoService;
    private readonly ICategoryService _categoryService;

    public CategoryController(IBlogPostService blogPostService, ICategoryService categoryService, ISeoService seoService)
    {
        _blogPostService = blogPostService;
        _categoryService = categoryService;
        _seoService = seoService;
    }

    // /oyun
    // /yazilim
    [HttpGet("{categorySlug}")]
    public IActionResult Index(string categorySlug, int page = 1)
    {
        // 1️⃣ KATEGORİ VAR MI?
        var category = _categoryService
            .GetAll()
            .FirstOrDefault(x => x.CategorySlug == categorySlug);

        if (category == null)
            return NotFound();
        
        int pageSize = 5;

        // 2️⃣ BU KATEGORİYE AİT YAZILAR
        var allPosts = _blogPostService
            .TGetCategoryWithBlogPosts()
            .Where(x =>
                x.Category != null &&
                x.Category.CategorySlug == categorySlug && x.Status == true)
            .ToList();

        var posts = allPosts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // 3️⃣ KATEGORİ ADI (yazı olmasa bile)
        ViewBag.CategoryName = category.CategoryName;
        ViewBag.CurrentPage = page;
        ViewBag.Pagesize = allPosts.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(allPosts.Count / (double)pageSize);
        ViewBag.CategorySlug = categorySlug;
        
        _seoService.SetCategorySeo(ViewData, category, allPosts.FirstOrDefault()?.ImageUrl);


        return View(posts);
    }

    [HttpGet("kategoriler")]
    public IActionResult kategoriler()
    {
        var value = _categoryService.GetAll().Where(x=>x.IsStatus == true).ToList();
        _seoService.SetCategoryListSeo(ViewData);
        return View(value);
    }


}