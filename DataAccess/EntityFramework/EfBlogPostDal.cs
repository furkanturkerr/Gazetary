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
}