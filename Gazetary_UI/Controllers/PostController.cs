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
    private readonly ISeoService _seoService;
    private readonly ICommentService _commentService;
    private readonly ICommentLikeService _commentLikeService;
    private readonly UserManager<AppUser> _userManager;

    public PostController(
        IBlogPostService blogPostService,
        ICommentService commentService,
        ICommentLikeService commentLikeService,
        UserManager<AppUser> userManager, ISeoService seoService)
    {
        _blogPostService = blogPostService;
        _commentService = commentService;
        _commentLikeService = commentLikeService;
        _userManager = userManager;
        _seoService = seoService;
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

        var viewCookieKey = $"viewed_post_{post.BlogPostId}";

        if (!Request.Cookies.ContainsKey(viewCookieKey))
        {
            _blogPostService.IncrementViewCount(post.BlogPostId);

            Response.Cookies.Append(viewCookieKey, "1", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(6),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Secure = Request.IsHttps
            });
        }

        var comments = _commentService.GetCommentsByBlogPostId(post.BlogPostId);

        var currentUser = await _userManager.GetUserAsync(User);

        var commentIds = comments.Select(x => x.CommentId).ToList();

        var likeCounts = commentIds.Any()
            ? _commentLikeService.GetLikeCountsByCommentIds(commentIds)
            : new Dictionary<int, int>();

        var likedIds = currentUser != null && commentIds.Any()
            ? _commentLikeService.GetLikedCommentIdsByUser(commentIds, currentUser.Id)
            : new List<int>();

        var model = new BlogDetailViewModel
        {
            BlogPost = post,
            Comments = comments,
            NewComment = new(),
            LikeCounts = likeCounts,
            LikedCommentIds = likedIds
        };
        
        _seoService.SetPostSeo(ViewData, post);


        return View(model);
    }
}