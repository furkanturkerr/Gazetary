using Business.Abstract;
using BlogProject.Models;
using Entities.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

[Route("{categorySlug}")]
public class PostController : Controller
{
    private readonly IBlogPostService _blogPostService;
    private readonly ICommentService _commentService;
    private readonly ICommentLikeService _commentLikeService;
    private readonly UserManager<AppUser> _userManager;

    public PostController(
        IBlogPostService blogPostService,
        ICommentService commentService,
        ICommentLikeService commentLikeService,
        UserManager<AppUser> userManager)
    {
        _blogPostService = blogPostService;
        _commentService = commentService;
        _commentLikeService = commentLikeService;
        _userManager = userManager;
    }

    [HttpGet("{postSlug}")]
    public async Task<IActionResult> Detail(string categorySlug, string postSlug)
    {
        var post = _blogPostService
            .TGetCategoryWithBlogPosts()
            .FirstOrDefault(x =>
                x.Category.CategorySlug == categorySlug &&
                x.Slug == postSlug);

        if (post == null)
            return NotFound();

        _blogPostService.IncrementViewCountAsync(post.BlogPostId);

        var comments = _commentService.GetAll()
            .Where(c => c.BlogPostId == post.BlogPostId)
            .OrderByDescending(c => c.CreatedDate)
            .ToList();

        var currentUser = await _userManager.GetUserAsync(User);

        var likeCounts = new Dictionary<int, int>();
        var likedIds = new List<int>();

        foreach (var comment in comments)
        {
            likeCounts[comment.CommentId] = _commentLikeService.GetLikeCount(comment.CommentId);

            if (currentUser != null)
            {
                var liked = _commentLikeService.GetByCommentAndUser(comment.CommentId, currentUser.Id);
                if (liked != null)
                    likedIds.Add(comment.CommentId);
            }
        }

        var model = new BlogDetailViewModel
        {
            BlogPost = post,
            Comments = comments,
            NewComment = new(),
            LikeCounts = likeCounts,
            LikedCommentIds = likedIds
        };

        return View(model);
    }
}