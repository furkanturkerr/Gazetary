using Business.Abstract;
using DataAccess.Abstarct;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Home;

public class _HomeCategoryWithTurkiye : ViewComponent
{
    private readonly IBlogPostService _blogPostService;

    public _HomeCategoryWithTurkiye(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var value = await _blogPostService.TGetBlogsWithCategoryByNameAsync("Son Dakika");
        return View(value);
    }
}