using DataAccess.Abstarct;
using Entities.Concrate;

namespace DataAccess.Abstract;

public interface ICommentLikeDal : IGenericDal<CommentLike>
{
    CommentLike? GetByCommentAndUser(int commentId, string userId);
    int GetLikeCount(int commentId);
    Dictionary<int, int> GetLikeCountsByCommentIds(List<int> commentIds);
    List<int> GetLikedCommentIdsByUser(List<int> commentIds, string userId);
    List<CommentLike> GetByCommentId(int commentId);
}