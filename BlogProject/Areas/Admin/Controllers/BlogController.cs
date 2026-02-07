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

    public IActionResult Index(int page = 1, int pageSize = 10)
    {
        var allPosts = _blogPostService.GetAll();
        
        var totalItems = allPosts.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        
        var posts = allPosts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalItems = totalItems;
        ViewBag.PageSize = pageSize;

        return View(posts);
    }
}