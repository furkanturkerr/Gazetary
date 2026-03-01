using Business.Abstract;
using DataAccess.Abstarct;
using Entities.Concrate;

namespace Business.Concrate;

public class CommentManager : ICommentService
{
    private readonly ICommentDal _commentDal;

    public CommentManager(ICommentDal commentDal)
    {
        _commentDal = commentDal;
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
}