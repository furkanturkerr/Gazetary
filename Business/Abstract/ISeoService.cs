using Entities.Concrate;

namespace Business.Abstract;

public interface ISeoService
{
    void SetPostSeo(IDictionary<string, object?> viewData, BlogPost post);
    void SetCategorySeo(IDictionary<string, object?> viewData, Category category, string? ogImage = null);
    void SetCategoryListSeo(IDictionary<string, object?> viewData);
}