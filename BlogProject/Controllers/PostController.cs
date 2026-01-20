using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

[Route("{categorySlug}")]
public class PostController : Controller
{
    private readonly IBlogPostService _blogPostService;

    public PostController(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    // /oyun/gta-6
    // /yazilim/asp-net-core
    [HttpGet("{postSlug}")]
    public IActionResult Detail(string categorySlug, string postSlug)
    {
        var post = _blogPostService
            .TGetCategoryWithBlogPosts()
            .FirstOrDefault(x =>
                x.Category.CategorySlug == categorySlug &&
                x.Slug == postSlug);

        if (post == null)
            return NotFound();

        return View(post);
    }
}