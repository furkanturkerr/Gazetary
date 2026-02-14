using DataAccess.Abstarct;
using DataAccess.Concrate;
using DataAccess.Repository;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework;

public class EfCategoryDal : GenericRepository<Category>, ICategoryDal
{
    private readonly Context _context;
    
    public EfCategoryDal(Context context) : base(context)
    {
        _context = context;
    }

    public void ChangeStatus(int id)
    {
        var value = _context.Categories.Find(id);
        if (value.IsStatus == true)
        {
            value.IsStatus = false;
            _context.SaveChanges();
        }
        else
        {
            value.IsStatus = true;
            _context.SaveChanges();
        }
    }
}