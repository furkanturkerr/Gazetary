using DataAccess.Abstarct;
using DataAccess.Concrate;
using DataAccess.Repository;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework;

public class EfBlogPostDal : GenericRepository<BlogPost>, IBlogPostDal
{
    private readonly Context _context;
    
    public EfBlogPostDal(Context context) : base(context)
    {
        _context = context;
    }

    public List<BlogPost> GetBlogsWithCategory()
    {
        // Kategorileri getirirken her bir kategorinin BlogPosts listesini de doldurur
        return _context.BlogPosts.Include(x => x.Category).ToList();
    }

    public void ChangeStatus(int id)
    {
        var value = _context.BlogPosts.Find(id);
        if (value.Status == true)
        {
            value.Status = false;
            _context.SaveChanges();
        }
        else
        {
            value.Status = true;
            _context.SaveChanges();
        }
    }

    public async Task<List<BlogPost>> GetBlogsWithCategoryByNameAsync(string categoryName)
    {
        return await _context.BlogPosts.Include(x => x.Category).Where(x => x.Category.CategoryName == categoryName).ToListAsync();
    }
    
    public async Task<List<BlogPost>> GetMostViewedBlogsAsync(int count)
    {
        var weekAgo = DateTime.Today.AddDays(-7);
        return await _context.BlogPosts
            .Include(x => x.Category)
            .Where(x => x.CreatedDate >= weekAgo)
            .OrderByDescending(x => x.ViewCount)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<BlogPost>> GetTodaysBlogsAsync()
    {
        var today = DateTime.Today;
        return await _context.BlogPosts
            .Include(x => x.Category)
            .Where(x => x.CreatedDate.Date == today)
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<BlogPost>> GetLatestBlogsByCategoryAsync(string categoryName, int count)
    {
        return await _context.BlogPosts
            .Include(x => x.Category)
            .Where(x => x.Category.CategoryName == categoryName)
            .OrderByDescending(x => x.CreatedDate)
            .Take(count)
            .ToListAsync();
    }
}