using Entities.Concrate;

namespace DataAccess.Abstarct;

public interface IBlogPostDal : IGenericDal<BlogPost>
{
    List<BlogPost> GetBlogsWithCategory();
    void ChangeStatus(int id);
}