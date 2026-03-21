using Entities.Concrate;

namespace Business.Abstract;

public interface IBlogPostService : IGenericService<BlogPost>
{
    public List<BlogPost> TGetCategoryWithBlogPosts();
    void ChangeStatus(int id);
    Task<List<BlogPost>> TGetBlogsWithCategoryByNameAsync(string categoryName);
    void IncrementViewCountAsync(int blogPostId);
    Task<List<BlogPost>> TGetMostViewedBlogsAsync(int count);
    Task<List<BlogPost>> TGetTodaysBlogsAsync();
    Task<List<BlogPost>> TGetLatestBlogsByCategoryAsync(string categoryName, int count);
}