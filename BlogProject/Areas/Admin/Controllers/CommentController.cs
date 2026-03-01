using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Areas.Admin.Controllers;

[Area("Admin")]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;
    private readonly IBlogPostService _blogPostService;

    public CommentController(ICommentService commentService, IBlogPostService blogPostService)
    {
        _commentService = commentService;
        _blogPostService = blogPostService;
    }

    public IActionResult CommentList(int page = 1, string postId = "")
    {
        int pageSize = 10;
        var allComments = _commentService.GetCommentsWithBlogPost();
        var comments = allComments.AsQueryable();

        if (!string.IsNullOrEmpty(postId) && int.TryParse(postId, out int pid))
            comments = comments.Where(x => x.BlogPostId == pid);

        var totalCount = comments.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var pagedComments = comments
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.AllPosts = allComments
            .GroupBy(x => x.BlogPostId)
            .Select(g => g.First().BlogPost)
            .Where(x => x != null)
            .OrderBy(x => x.Title)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.SelectedPost = postId;
        ViewBag.TotalCount = totalCount;

        return View(pagedComments);
    }}