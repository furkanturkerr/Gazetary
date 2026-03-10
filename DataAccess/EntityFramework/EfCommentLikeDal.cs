using DataAccess.Abstract;
using DataAccess.Concrate;
using DataAccess.Repository;
using Entities.Concrate;

namespace DataAccess.Concrate;

public class EfCommentLikeDal : GenericRepository<CommentLike>, ICommentLikeDal
{
    public EfCommentLikeDal(Context context) : base(context)
    {
    }
}