using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BlogProject.Controllers;

[Authorize]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;
    private readonly ICommentLikeService _commentLikeService;
    private readonly UserManager<AppUser> _userManager;

    public CommentController(
        ICommentService commentService,
        ICommentLikeService commentLikeService,
        UserManager<AppUser> userManager)
    {
        _commentService     = commentService;
        _commentLikeService = commentLikeService;
        _userManager        = userManager;
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [EnableRateLimiting("comment-limit")]
    public async Task<IActionResult> AddComment(Comment comment)
    {
        if (string.IsNullOrWhiteSpace(comment.Content))
            return Json(new { success = false, message = "Yorum boş olamaz." });

        if (comment.Content.Length > 1000)
            return Json(new { success = false, message = "Yorum en fazla 1000 karakter olabilir." });

        if (comment.BlogPostId <= 0)
            return Json(new { success = false, message = "Geçersiz yazı." });

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

        comment.Content     = comment.Content.Trim();
        comment.CreatedDate = DateTime.Now;
        comment.Name        = user.NameSurname;
        comment.AppUserId   = user.Id;
        comment.IsStatus    = true;

        _commentService.Insert(comment);

        return Json(new
        {
            success   = true,
            commentId = comment.CommentId,
            name      = comment.Name,
            content   = comment.Content,
            date      = comment.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            userId    = comment.AppUserId
        });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        if (commentId <= 0)
            return Json(new { success = false, message = "Geçersiz yorum." });

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

        var comment = _commentService.GetById(commentId);
        if (comment == null)
            return Json(new { success = false, message = "Yorum bulunamadı." });

        if (comment.AppUserId != user.Id)
            return Json(new { success = false, message = "Bu yorum size ait değil." });

        var likes = _commentLikeService.GetByCommentId(commentId);

        foreach (var like in likes)
            _commentLikeService.Delete(like);

        _commentService.Delete(comment);
        return Json(new { success = true });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [EnableRateLimiting("like-limit")]
    public async Task<IActionResult> ToggleLike(int commentId)
    {
        if (commentId <= 0)
            return Json(new { success = false, message = "Geçersiz yorum." });

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

        var comment = _commentService.GetById(commentId);
        if (comment == null)
            return Json(new { success = false, message = "Yorum bulunamadı." });

        var existing = _commentLikeService.GetByCommentAndUser(commentId, user.Id);

        if (existing != null)
        {
            _commentLikeService.Delete(existing);
            var count = _commentLikeService.GetLikeCount(commentId);
            return Json(new { success = true, liked = false, count });
        }

        _commentLikeService.Insert(new CommentLike
        {
            CommentId = commentId,
            AppUserId = user.Id
        });

        var newCount = _commentLikeService.GetLikeCount(commentId);
        return Json(new { success = true, liked = true, count = newCount });
    }
}