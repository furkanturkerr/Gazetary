using Entities.Concrate;

namespace Business.Abstract;

public interface IBlogPostService : IGenericService<BlogPost>
{
    public List<BlogPost> TGetCategoryWithBlogPosts();
    void ChangeStatus(int id);
}