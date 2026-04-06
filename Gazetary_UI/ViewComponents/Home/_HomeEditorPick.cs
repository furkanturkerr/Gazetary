using BlogProject.Models;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

public class _HomeEditorPick : ViewComponent
{
    private readonly IBlogPostService _blogPostService;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "viewcomponent_home_editorpick";
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5);

    public _HomeEditorPick(IBlogPostService blogPostService, IMemoryCache cache)
    {
        _blogPostService = blogPostService;
        _cache = cache;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheExpiry;

            var todayBlogs = await _blogPostService.TGetTodaysBlogsAsync();

            if (todayBlogs == null || !todayBlogs.Any())
            {
                todayBlogs = _blogPostService.TGetCategoryWithBlogPosts()
                    ?.OrderByDescending(x => x.CreatedDate)
                    .Take(5)
                    .ToList();
            }

            var ekonomiPosts = await _blogPostService.TGetLatestBlogsByCategoryAsync("Ekonomi", 1);
            var sporPosts = await _blogPostService.TGetLatestBlogsByCategoryAsync("Spor", 1);
            var teknolojiPosts = await _blogPostService.TGetLatestBlogsByCategoryAsync("Teknoloji", 1);
            var turkiyePosts    = await _blogPostService.TGetLatestBlogsByCategoryAsync("Son Dakika",   1);

            return new HomeEditorPickViewModel
            {
                TodaySliderPosts = todayBlogs,
                Category1 = new CategoryWithLatestBlog
                {
                    CategoryName = "Ekonomi",
                    CategorySlug = "ekonomi",
                    Posts = ekonomiPosts
                },
                Category2 = new CategoryWithLatestBlog
                {
                    CategoryName = "Spor",
                    CategorySlug = "spor",
                    Posts = sporPosts
                },
                Category3 = new CategoryWithLatestBlog
                {
                    CategoryName = "Teknoloji",
                    CategorySlug = "teknoloji",
                    Posts = teknolojiPosts
                },

                Category4 = new CategoryWithLatestBlog
                {
                    CategoryName = "Son Dakika",
                    CategorySlug = "turkiye",
                    Posts = turkiyePosts
                }
            };
        });

        return View(viewModel);
    }
}