using DataAccess.Abstarct;
using DataAccess.Concrate;
using DataAccess.Repository;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework;

public class EfCommentDal : GenericRepository<Comment>, ICommentDal
{
    private readonly Context _context;
    public EfCommentDal(Context context) : base(context)
    {
        _context = context;
    }

    public List<Comment> GetCommentsWithBlogPost(int id)
    {
        var value = _context.Comments.Include(x => x.BlogPost).Where(x => x.BlogPostId == id).ToList();
        return value;
    }

    public List<Comment> GetCommentsWithBlogPost()
    {
        var value = _context.Comments.Include(x => x.BlogPost);
        return value.ToList();
    }

    public List<Comment> GetCommentsByBlogPostId(int blogPostId)
    {
        return _context.Comments
            .Where(x => x.BlogPostId == blogPostId && x.IsStatus)
            .OrderByDescending(x => x.CreatedDate)
            .ToList();
    }
}