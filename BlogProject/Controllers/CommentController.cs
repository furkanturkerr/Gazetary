using BlogProject.Models;
using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public IActionResult AddComment(Comment comment)
    {
        comment.CreatedDate = DateTime.Now;
        comment.Name = "furkan"; // identity gelecek

        _commentService.Insert(comment);

        return Json(new
        {
            success = true,
            name = comment.Name,
            content = comment.Content,
            date = comment.CreatedDate.ToString("dd.MM.yyyy HH:mm")
        });
    }
}