using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Abstarct;

public interface ICommentDal : IGenericDal<Comment>
{
    List<Comment> GetCommentsWithBlogPost(int id);
    List<Comment> GetCommentsWithBlogPost();
    List<Comment> GetCommentsByBlogPostId(int blogPostId);
}