using Business.Abstract;
using DataAccess.Abstarct;
using Entities.Concrate;

namespace Business.Concrate;

public class BlogPostManager : IBlogPostService
{
    private readonly IBlogPostDal _blogPostDal;

    public BlogPostManager(IBlogPostDal blogPostDal)
    {
        _blogPostDal = blogPostDal;
    }

    public void Insert(BlogPost t)
    {
        _blogPostDal.Insert(t);
    }

    public void Update(BlogPost t)
    {
        _blogPostDal.Update(t);
    }

    public void Delete(BlogPost t)
    {
        _blogPostDal.Delete(t);
    }

    public List<BlogPost> GetAll()
    {
        return _blogPostDal.GetAll();
    }

    public BlogPost GetById(int id)
    {
        return _blogPostDal.GetById(id);
    }

    public List<BlogPost> TGetCategoryWithBlogPosts()
    {
        return _blogPostDal.GetBlogsWithCategory();
    }

    public void ChangeStatus(int id)
    {
        _blogPostDal.ChangeStatus(id);
    }

    public async Task<List<BlogPost>> TGetBlogsWithCategoryByNameAsync(string categoryName)
    {
        return await _blogPostDal.GetBlogsWithCategoryByNameAsync(categoryName);
    }
    
    public void IncrementViewCountAsync(int blogPostId)
    {
        var blog = _blogPostDal.GetById(blogPostId);
        if (blog != null)
        {
            blog.ViewCount++;
            _blogPostDal.Update(blog);
        }
    }
    
    public async Task<List<BlogPost>> TGetMostViewedBlogsAsync(int count)
    {
        return await _blogPostDal.GetMostViewedBlogsAsync(count);
    }

    public async Task<List<BlogPost>> TGetTodaysBlogsAsync()
    {
        return await _blogPostDal.GetTodaysBlogsAsync();
    }

    public async Task<List<BlogPost>> TGetLatestBlogsByCategoryAsync(string categoryName, int count)
    {
        return await _blogPostDal.GetLatestBlogsByCategoryAsync(categoryName, count);
    }
}