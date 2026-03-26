using DataAccess.Abstract;
using DataAccess.Concrate;
using DataAccess.Repository;
using Entities.Concrate;

namespace DataAccess.EntityFramework;

public class EfCommentLikeDal : GenericRepository<CommentLike>, ICommentLikeDal
{
    private readonly Context _context;

    public EfCommentLikeDal(Context context) : base(context)
    {
        _context = context;
    }

    public CommentLike? GetByCommentAndUser(int commentId, string userId)
    {
        return _context.CommentLikes
            .FirstOrDefault(x => x.CommentId == commentId && x.AppUserId == userId);
    }

    public int GetLikeCount(int commentId)
    {
        return _context.CommentLikes
            .Count(x => x.CommentId == commentId);
    }

    public Dictionary<int, int> GetLikeCountsByCommentIds(List<int> commentIds)
    {
        return _context.CommentLikes
            .Where(x => commentIds.Contains(x.CommentId))
            .GroupBy(x => x.CommentId)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public List<int> GetLikedCommentIdsByUser(List<int> commentIds, string userId)
    {
        return _context.CommentLikes
            .Where(x => commentIds.Contains(x.CommentId) && x.AppUserId == userId)
            .Select(x => x.CommentId)
            .ToList();
    }

    public List<CommentLike> GetByCommentId(int commentId)
    {
        return _context.CommentLikes
            .Where(x => x.CommentId == commentId)
            .ToList();
    }
}