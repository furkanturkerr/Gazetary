using Entities.Concrate;

namespace Business.Abstract;

public interface ICommentLikeService : IGenericService<CommentLike>
{
    CommentLike? GetByCommentAndUser(int commentId, string userId);
    int GetLikeCount(int commentId);
    Dictionary<int, int> GetLikeCountsByCommentIds(List<int> commentIds);
    List<int> GetLikedCommentIdsByUser(List<int> commentIds, string userId);
    List<CommentLike> GetByCommentId(int commentId);
}