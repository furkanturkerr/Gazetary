using DataAccess.Abstarct;
using DataAccess.Concrate;
using DataAccess.Repository;
using Entities.Concrate;

namespace DataAccess.EntityFramework;

public class EfContactDal : GenericRepository<Contact>, IContactDal
{
    public EfContactDal(Context context) : base(context)
    {
    }
}