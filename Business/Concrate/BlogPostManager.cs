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
        throw new NotImplementedException();
    }

    public void Update(BlogPost t)
    {
        throw new NotImplementedException();
    }

    public void Delete(BlogPost t)
    {
        throw new NotImplementedException();
    }

    public List<BlogPost> GetAll()
    {
        return _blogPostDal.GetAll();
    }

    public BlogPost GetById(int id)
    {
        throw new NotImplementedException();
    }

    public List<BlogPost> TGetCategoryWithBlogPosts()
    {
        return _blogPostDal.GetBlogsWithCategory();
    }
}