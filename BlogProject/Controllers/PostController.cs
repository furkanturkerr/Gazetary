using Business.Abstract;
using BlogProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

[Route("{categorySlug}")]
public class PostController : Controller
{
    private readonly IBlogPostService _blogPostService;
    private readonly ICommentService _commentService;

    public PostController(IBlogPostService blogPostService, ICommentService commentService)
    {
        _blogPostService = blogPostService;
        _commentService = commentService;
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

        var comments = _commentService.GetAll()
            .Where(c => c.BlogPostId == post.BlogPostId)
            .ToList();

        var model = new BlogDetailViewModel
        {
            BlogPost = post,
            Comments = comments,
            NewComment = new()
        };

        return View(model);
    }
}
