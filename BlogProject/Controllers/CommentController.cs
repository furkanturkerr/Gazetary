using BlogProject.Models;
using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

[Authorize]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;
    private readonly ICommentLikeService _commentLikeService;
    private readonly UserManager<AppUser> _userManager;

    public CommentController(ICommentService commentService, ICommentLikeService commentLikeService, UserManager<AppUser> userManager)
    {
        _commentService = commentService;
        _commentLikeService = commentLikeService;
        _userManager = userManager;
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> AddComment(Comment comment)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

        comment.CreatedDate = DateTime.Now;
        comment.Name = user.NameSurname;
        comment.AppUserId = user.Id;
        comment.IsStatus = true;

        _commentService.Insert(comment);

        return Json(new
        {
            success = true,
            commentId = comment.CommentId,
            name = comment.Name,
            content = comment.Content,
            date = comment.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
            userId = comment.AppUserId,
            parentCommentId = comment.ParentCommentId
        });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

        var comment = _commentService.GetById(commentId);
        if (comment == null)
            return Json(new { success = false, message = "Yorum bulunamadı." });

        if (comment.AppUserId == null || comment.AppUserId != user.Id)
            return Json(new { success = false, message = "Bu yorum size ait değil." });

        _commentService.Delete(comment);
        return Json(new { success = true });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> ToggleLike(int commentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

        var existing = _commentLikeService.GetByCommentAndUser(commentId, user.Id);

        if (existing != null)
        {
            _commentLikeService.Delete(existing);
            var count = _commentLikeService.GetLikeCount(commentId);
            return Json(new { success = true, liked = false, count });
        }
        else
        {
            _commentLikeService.Insert(new CommentLike
            {
                CommentId = commentId,
                AppUserId = user.Id
            });
            var count = _commentLikeService.GetLikeCount(commentId);
            return Json(new { success = true, liked = true, count });
        }
    }
}