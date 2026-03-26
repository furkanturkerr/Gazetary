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

[Route("sitemap.xml")]
public IActionResult Sitemap()
{
    var posts = _blogPostService.TGetCategoryWithBlogPosts()
        .Where(x => x.Status)
        .OrderByDescending(x => x.CreatedDate)
        .ToList();

    var categories = posts
        .Select(x => x.Category)
        .DistinctBy(x => x.CategoryId)
        .ToList();

    var sb = new System.Text.StringBuilder();
    sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
    sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

    sb.AppendLine("  <url>");
    sb.AppendLine("    <loc>https://gazetary.com</loc>");
    sb.AppendLine("    <changefreq>daily</changefreq>");
    sb.AppendLine("    <priority>1.0</priority>");
    sb.AppendLine("  </url>");

    foreach (var page in new[] { "hakkimizda", "iletisim", "reklam", "kategoriler" })
    {
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>https://gazetary.com/{page}</loc>");
        sb.AppendLine("    <changefreq>monthly</changefreq>");
        sb.AppendLine("    <priority>0.5</priority>");
        sb.AppendLine("  </url>");
    }

    foreach (var category in categories)
    {
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>https://gazetary.com/{category.CategorySlug}</loc>");
        sb.AppendLine("    <changefreq>daily</changefreq>");
        sb.AppendLine("    <priority>0.7</priority>");
        sb.AppendLine("  </url>");
    }

    foreach (var post in posts)
    {
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>https://gazetary.com/{post.Category.CategorySlug}/{post.Slug}</loc>");
        sb.AppendLine($"    <lastmod>{post.CreatedDate:yyyy-MM-dd}</lastmod>");
        sb.AppendLine("    <changefreq>weekly</changefreq>");
        sb.AppendLine("    <priority>0.8</priority>");
        sb.AppendLine("  </url>");
    }

    sb.AppendLine("</urlset>");

    return Content(sb.ToString(), "application/xml", System.Text.Encoding.UTF8);
}
}