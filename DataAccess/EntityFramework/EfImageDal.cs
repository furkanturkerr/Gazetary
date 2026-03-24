using DataAccess.Abstarct;
using DataAccess.Concrate;
using DataAccess.Repository;
using Entities.Concrate;

namespace DataAccess.EntityFramework;

public class EfImageDal : GenericRepository<Image>, IImageDal
{
    public EfImageDal(Context context) : base(context)
    {
    }
}