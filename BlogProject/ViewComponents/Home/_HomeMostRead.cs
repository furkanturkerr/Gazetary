using BlogProject.Models;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.ViewComponents.Home;

public class _HomeMostRead : ViewComponent
{
    private readonly IBlogPostService _blogPostService;
    private readonly ICategoryService _categoryService;

    public _HomeMostRead(IBlogPostService blogPostService, ICategoryService categoryService)
    {
        _blogPostService = blogPostService;
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var mostViewed = await _blogPostService.TGetMostViewedBlogsAsync(5);

        if (!mostViewed.Any())
        {
            mostViewed = _blogPostService.TGetCategoryWithBlogPosts().Take(5).ToList();
        }

        var allPosts = _blogPostService.GetAll().ToList();

        var allCategories = _categoryService.GetAll()
            .Select(c => new CategorySummary
            {
                CategoryName = c.CategoryName,
                CategorySlug = c.CategorySlug,
                BlogCount    = allPosts.Count(p => p.CategoryId == c.CategoryId)
            })
            .OrderByDescending(c => c.BlogCount)
            .Take(5)
            .ToList();

        var viewModel = new HomeMostReadViewModel
        {
            MostReadPosts = mostViewed,
            Categories    = allCategories
        };

        return View(viewModel);
    }
}