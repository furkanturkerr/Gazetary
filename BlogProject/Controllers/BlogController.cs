using Business.Abstract;
using DataAccess.Concrate;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

public class BlogController : Controller
{
    private readonly IBlogPostService _blogPostService;

    public BlogController(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var blogPosts = _blogPostService.TGetCategoryWithBlogPosts();
        return View(blogPosts);
    }
    
    [HttpGet]
    public IActionResult Detail(string slug)
    {
        var blog = _blogPostService.TGetCategoryWithBlogPosts().FirstOrDefault(x => x.Slug == slug);

        if (blog == null)
            return NotFound();

        return View(blog);
    }
}