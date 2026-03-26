using Business.Abstract;
using DataAccess.Abstarct;
using Entities.Concrate;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Concrate;

public class BlogPostManager : IBlogPostService
{
    private readonly IBlogPostDal _blogPostDal;
    private readonly IMemoryCache _cache;

    private const string CacheKeyAll          = "blogposts_all";
    private const string CacheKeyWithCategory = "blogposts_with_category";
    private const string CacheKeyMostViewed   = "blogposts_most_viewed_{0}";
    private const string CacheKeyTodays       = "blogposts_todays";
    private const string CacheKeyByCategory   = "blogposts_category_{0}_{1}";

    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan TodaysExpiry  = TimeSpan.FromMinutes(5);

    public BlogPostManager(IBlogPostDal blogPostDal, IMemoryCache cache)
    {
        _blogPostDal = blogPostDal;
        _cache       = cache;
    }

    public List<BlogPost> GetAll()
    {
        return _cache.GetOrCreate<List<BlogPost>>(CacheKeyAll, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = DefaultExpiry;
            return _blogPostDal.GetAll();
        })!;
    }

    public BlogPost GetById(int id)
    {
        return _blogPostDal.GetById(id);
    }

    public void Insert(BlogPost entity)
    {
        _blogPostDal.Insert(entity);
        InvalidateCache();
    }

    public void Update(BlogPost entity)
    {
        _blogPostDal.Update(entity);
        InvalidateCache();
    }

    public void Delete(BlogPost entity)
    {
        _blogPostDal.Delete(entity);
        InvalidateCache();
    }

    public List<BlogPost> TGetCategoryWithBlogPosts()
    {
        return _cache.GetOrCreate<List<BlogPost>>(CacheKeyWithCategory, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = DefaultExpiry;
            return _blogPostDal.GetBlogsWithCategory();
        })!;
    }

    public async Task<List<BlogPost>> TGetTodaysBlogsAsync()
    {
        return (await _cache.GetOrCreateAsync<List<BlogPost>>(CacheKeyTodays, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TodaysExpiry;
            return await _blogPostDal.GetTodaysBlogsAsync();
        }))!;
    }

    public async Task<List<BlogPost>> TGetMostViewedBlogsAsync(int count)
    {
        var key = string.Format(CacheKeyMostViewed, count);
        return (await _cache.GetOrCreateAsync<List<BlogPost>>(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = DefaultExpiry;
            return await _blogPostDal.GetMostViewedBlogsAsync(count);
        }))!;
    }

    public async Task<List<BlogPost>> TGetLatestBlogsByCategoryAsync(string categoryName, int count)
    {
        var key = string.Format(CacheKeyByCategory, categoryName.ToLower(), count);
        return (await _cache.GetOrCreateAsync<List<BlogPost>>(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = DefaultExpiry;
            return await _blogPostDal.GetLatestBlogsByCategoryAsync(categoryName, count);
        }))!;
    }

    public async Task<List<BlogPost>> TGetBlogsWithCategoryByNameAsync(string categoryName)
    {
        var key = $"blogposts_byname_{categoryName.ToLower()}";
        return (await _cache.GetOrCreateAsync<List<BlogPost>>(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = DefaultExpiry;
            return await _blogPostDal.GetBlogsWithCategoryByNameAsync(categoryName);
        }))!;
    }

    public void ChangeStatus(int id)
    {
        _blogPostDal.ChangeStatus(id);
        InvalidateCache();
    }

    public void IncrementViewCount(int blogPostId)
    {
        var post = _blogPostDal.GetById(blogPostId);
        if (post == null) return;

        post.ViewCount++;
        _blogPostDal.Update(post);

        _cache.Remove(string.Format(CacheKeyMostViewed, 4));
        _cache.Remove(string.Format(CacheKeyMostViewed, 5));
        _cache.Remove(string.Format(CacheKeyMostViewed, 10));
    }

    private void InvalidateCache()
    {
        _cache.Remove(CacheKeyAll);
        _cache.Remove(CacheKeyWithCategory);
        _cache.Remove(CacheKeyTodays);

        foreach (var count in new[] { 1, 2, 3, 4, 5, 10 })
            _cache.Remove(string.Format(CacheKeyMostViewed, count));
    }
}