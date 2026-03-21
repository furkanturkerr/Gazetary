using Entities.Concrate;

namespace DataAccess.Abstarct;

public interface IBlogPostDal : IGenericDal<BlogPost>
{
    List<BlogPost> GetBlogsWithCategory();
    void ChangeStatus(int id);
    Task<List<BlogPost>> GetMostViewedBlogsAsync(int count);
    Task<List<BlogPost>> GetBlogsWithCategoryByNameAsync(string categoryName);
    Task<List<BlogPost>> GetTodaysBlogsAsync();
    Task<List<BlogPost>> GetLatestBlogsByCategoryAsync(string categoryName, int count);
}