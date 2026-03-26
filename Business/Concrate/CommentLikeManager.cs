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

    public void Insert(CommentLike like)
        => _commentLikeDal.Insert(like);

    public void Update(CommentLike t)
        => _commentLikeDal.Update(t);

    public void Delete(CommentLike like)
        => _commentLikeDal.Delete(like);

    public List<CommentLike> GetAll()
        => _commentLikeDal.GetAll();

    public CommentLike GetById(int id)
        => _commentLikeDal.GetById(id);

    public CommentLike? GetByCommentAndUser(int commentId, string userId)
        => _commentLikeDal.GetByCommentAndUser(commentId, userId);

    public int GetLikeCount(int commentId)
        => _commentLikeDal.GetLikeCount(commentId);

    public Dictionary<int, int> GetLikeCountsByCommentIds(List<int> commentIds)
        => _commentLikeDal.GetLikeCountsByCommentIds(commentIds);

    public List<int> GetLikedCommentIdsByUser(List<int> commentIds, string userId)
        => _commentLikeDal.GetLikedCommentIdsByUser(commentIds, userId);

    public List<CommentLike> GetByCommentId(int commentId)
        => _commentLikeDal.GetByCommentId(commentId);
}