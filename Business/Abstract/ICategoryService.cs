using Entities.Concrate;

namespace Business.Abstract;

public interface ICategoryService : IGenericService<Category>
{
    void TChangeStatus(int id);
}