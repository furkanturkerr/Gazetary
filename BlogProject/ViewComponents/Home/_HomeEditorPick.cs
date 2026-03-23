using BlogProject.Models;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

public class _HomeEditorPick : ViewComponent
{
    private readonly IBlogPostService _blogPostService;

    public _HomeEditorPick(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
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

        var viewModel = new HomeEditorPickViewModel
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

        return View(viewModel);
    }
}