using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrate;

namespace Business.Concrate;

public class CommentLikeManager : ICommentLikeService
{
    private readonly ICommentLikeDal _commentLikeDal;

    public CommentLikeManager(ICommentLikeDal commentLikeDal)
    {
        _commentLikeDal = commentLikeDal;
    }

    public CommentLike? GetByCommentAndUser(int commentId, string userId)
        => _commentLikeDal.GetAll()
            .FirstOrDefault(x => x.CommentId == commentId && x.AppUserId == userId);

    public void Insert(CommentLike like)
        => _commentLikeDal.Insert(like);

    public void Delete(CommentLike like)
        => _commentLikeDal.Delete(like);

    public int GetLikeCount(int commentId)
        => _commentLikeDal.GetAll()
            .Count(x => x.CommentId == commentId);
}