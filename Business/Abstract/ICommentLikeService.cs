using Entities.Concrate;

namespace Business.Abstract;

public interface ICommentLikeService : IGenericService<CommentLike>
{
    CommentLike? GetByCommentAndUser(int commentId, string userId);
    void Insert(CommentLike like);
    void Delete(CommentLike like);
    int GetLikeCount(int commentId);
}