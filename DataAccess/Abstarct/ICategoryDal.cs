using Entities.Concrate;

namespace DataAccess.Abstarct;

public interface ICategoryDal : IGenericDal<Category>
{
    void ChangeStatus(int id);
}