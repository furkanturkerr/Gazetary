using Business.Abstract;
using DataAccess.Abstarct;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Home;

public class _HomeCategoryWithHistory : ViewComponent
{
    private readonly IBlogPostService _blogPostService;

    public _HomeCategoryWithHistory(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var value = await _blogPostService.TGetBlogsWithCategoryByNameAsync("Tarih");
        return View(value);
    }
}