using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

public class DefaultController : Controller
{
    private readonly IBlogPostService _blogPostService;

    public DefaultController(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    // GET
    public IActionResult Anasayfa()
    {
        return View();
    }
    
    [HttpGet]
    [Route("Default/Search")]
    public IActionResult Search(string q)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
            return Json(new { results = new List<object>() });
 
        var results = _blogPostService.TGetCategoryWithBlogPosts()
            .Where(x => x.Status &&
                        (x.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                         x.Description.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                         x.Category.CategoryName.Contains(q, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(x => x.CreatedDate)
            .Take(6)
            .Select(x => new
            {
                title        = x.Title,
                slug         = x.Slug,
                categoryName = x.Category.CategoryName,
                categorySlug = x.Category.CategorySlug,
                imageUrl     = x.ImageUrl,
                date         = x.CreatedDate.ToString("dd MMM yyyy")
            })
            .ToList();
 
        return Json(new { results });
    }
 
    [HttpGet]
    [Route("arama")]
    public IActionResult SearchPage(string q)
    {
        var results = new List<Entities.Concrate.BlogPost>();
 
        if (!string.IsNullOrWhiteSpace(q) && q.Length >= 2)
        {
            results = _blogPostService.TGetCategoryWithBlogPosts()
                .Where(x => x.Status &&
                            (x.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                             x.Description.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                             x.Category.CategoryName.Contains(q, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
        }
 
        ViewBag.Query   = q;
        ViewBag.Count   = results.Count;
        return View(results);
    }
}