using Business.Abstract;
using DataAccess.Abstarct;
using Entities.Concrate;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Concrate;

public class CommentManager : ICommentService
{
    private readonly ICommentDal _commentDal;
    private readonly IMemoryCache _cache;

    private const string CacheKeyByBlogPost = "comments_blogpost_{0}";
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5);

    public CommentManager(ICommentDal commentDal, IMemoryCache cache)
    {
        _commentDal = commentDal;
        _cache = cache;
    }

    public void Insert(Comment t)
    {
        _commentDal.Insert(t);
    }

    public void Update(Comment t)
    {
        _commentDal.Update(t);
    }

    public void Delete(Comment t)
    {
        _commentDal.Delete(t);
    }

    public List<Comment> GetAll()
    {
        return _commentDal.GetAll();
    }

    public Comment GetById(int id)
    {
        return _commentDal.GetById(id);
    }

    public List<Comment> GetCommentsWithBlogPost(int id)
    {
        return _commentDal.GetCommentsWithBlogPost(id);
    }

    public List<Comment> GetCommentsWithBlogPost()
    {
        return _commentDal.GetCommentsWithBlogPost();
    }

    public List<Comment> GetCommentsByBlogPostId(int blogPostId)
    {
        return _commentDal.GetCommentsByBlogPostId(blogPostId);
    }
}