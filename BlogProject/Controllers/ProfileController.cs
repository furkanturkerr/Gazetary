using BlogProject.Dtos;
using BlogProject.Models;
using Business.Abstract;
using Entities.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers;

[Route("profil")]
public class ProfileController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IBlogPostService _blogPostService;
    private readonly ICommentService _commentService;

    public ProfileController(
        UserManager<AppUser> userManager,
        IBlogPostService blogPostService,
        ICommentService commentService)
    {
        _userManager    = userManager;
        _blogPostService = blogPostService;
        _commentService  = commentService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Profile(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return View("~/Views/Shared/NotFound.cshtml");
        
        ViewBag.useremailstatus = user.EmailConfirmed;

        var userComments = _commentService.GetAll()
            .Where(c => c.AppUserId == id)
            .OrderByDescending(c => c.CreatedDate)
            .ToList();

        var mostRead = _blogPostService.TGetCategoryWithBlogPosts()
            .OrderByDescending(x => x.ViewCount)
            .Take(4)
            .ToList();

        var latest = _blogPostService.TGetCategoryWithBlogPosts()
            .OrderByDescending(x => x.CreatedDate)
            .Take(4)
            .ToList();

        var vm = new ProfileViewModel
        {
            User = new ProfileDto
            {
                Id          = user.Id,
                NameSurname = user.NameSurname,
                Email       = user.Email ?? string.Empty,
            },
            UserComments   = userComments,
            MostReadPosts  = mostRead,
            LatestPosts    = latest,
            TotalReadCount = userComments.Count * 28
        };

        return View(vm);
    }
}