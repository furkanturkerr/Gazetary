using BlogProject.Models;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BlogProject.ViewComponents.Home;

public class _HomeMostRead : ViewComponent
{
    private readonly IBlogPostService _blogPostService;
    private readonly ICategoryService _categoryService;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "viewcomponent_home_mostread";
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(10);

    public _HomeMostRead(IBlogPostService blogPostService, ICategoryService categoryService, IMemoryCache cache)
    {
        _blogPostService = blogPostService;
        _categoryService = categoryService;
        _cache = cache;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheExpiry;

            var mostViewed = await _blogPostService.TGetMostViewedBlogsAsync(10);

            if (!mostViewed.Any())
            {
                mostViewed = _blogPostService.TGetCategoryWithBlogPosts().Take(5).ToList();
            }

            var allPosts = _blogPostService.GetAll();
            var allCategories = _categoryService.GetAll();

            var categorySummaries = allCategories
                .Select(c => new CategorySummary
                {
                    CategoryName = c.CategoryName,
                    CategorySlug = c.CategorySlug,
                    BlogCount    = allPosts.Count(p => p.CategoryId == c.CategoryId)
                })
                .OrderByDescending(c => c.BlogCount)
                .Take(6)
                .ToList();

            return new HomeMostReadViewModel
            {
                MostReadPosts = mostViewed,
                Categories    = categorySummaries
            };
        });

        return View(viewModel);
    }
}