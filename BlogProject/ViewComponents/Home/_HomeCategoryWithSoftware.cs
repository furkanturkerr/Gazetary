using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Home;

public class _HomeCategoryWithSoftware : ViewComponent
{
    private readonly IBlogPostService _blogPostService;

    public _HomeCategoryWithSoftware(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var value = await _blogPostService.TGetBlogsWithCategoryByNameAsync("Yazılım");
        return View(value);
    }
}