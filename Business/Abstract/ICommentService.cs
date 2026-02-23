using Entities.Concrate;

namespace Business.Abstract;

public interface ICommentService : IGenericService<Comment>
{
    List<Comment> GetCommentsWithBlogPost(int id);
}