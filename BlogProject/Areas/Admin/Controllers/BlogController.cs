using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;

[Area("Admin")]
public class BlogController : Controller
{
    private readonly IBlogPostService _blogPostService;

    public BlogController(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    public IActionResult Index()
    {
        var allPosts = _blogPostService.TGetCategoryWithBlogPosts();
        return View(allPosts);
    }
}