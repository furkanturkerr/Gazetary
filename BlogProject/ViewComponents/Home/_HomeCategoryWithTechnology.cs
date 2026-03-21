using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Home;

public class _HomeCategoryWithTechnology : ViewComponent
{
    private readonly IBlogPostService _blogPostService;

    public _HomeCategoryWithTechnology(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var value = await _blogPostService.TGetBlogsWithCategoryByNameAsync("Teknoloji");
        return View(value);
    }
}