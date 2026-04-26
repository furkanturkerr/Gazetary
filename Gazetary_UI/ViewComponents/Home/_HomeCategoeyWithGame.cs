using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Home;

public class _HomeCategoeyWithGame : ViewComponent
{
    private readonly IBlogPostService _blogPostService;

    public _HomeCategoeyWithGame(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var value = await _blogPostService.TGetBlogsWithCategoryByNameAsync("oyun");
        return View(value);
    }
}