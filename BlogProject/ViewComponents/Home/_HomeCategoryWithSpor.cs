using BlogProject.Models;
using Business.Abstract;
using DataAccess.Abstarct;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Home;

public class _HomeCategoryWithSpor : ViewComponent
{
    private readonly IBlogPostService _blogPostService;

    public _HomeCategoryWithSpor(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var value = await _blogPostService.TGetBlogsWithCategoryByNameAsync("Spor");

        var viewModel = new CategoryBlogsViewModel()
        {
            LatestPost = value.OrderByDescending(x => x.CreatedDate).FirstOrDefault(),
            OtherPosts = value.OrderByDescending(x => x.CreatedDate).Skip(1).Take(3).ToList()
        };
        
        return View(viewModel);
    }
}