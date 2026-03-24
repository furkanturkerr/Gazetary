using Business.Abstract;
using DataAccess.Abstarct;
using Entities.Concrate;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Concrate;

public class CategoryManager : ICategoryService
{
    private readonly ICategoryDal _categoryDal;
    private readonly IMemoryCache _cache;

    private const string CacheKeyAll = "categories_all";
    private static readonly TimeSpan Expiry = TimeSpan.FromHours(1);

    public CategoryManager(ICategoryDal categoryDal, IMemoryCache cache)
    {
        _categoryDal = categoryDal;
        _cache       = cache;
    }

    public List<Category> GetAll()
    {
        return _cache.GetOrCreate<List<Category>>(CacheKeyAll, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = Expiry;
            return _categoryDal.GetAll();
        })!;
    }

    public Category GetById(int id)
    {
        return _categoryDal.GetById(id);
    }

    public void Insert(Category t)
    {
        _categoryDal.Insert(t);
        _cache.Remove(CacheKeyAll);
    }

    public void Update(Category t)
    {
        _categoryDal.Update(t);
        _cache.Remove(CacheKeyAll);
    }

    public void Delete(Category t)
    {
        _categoryDal.Delete(t);
        _cache.Remove(CacheKeyAll);
    }

    public void TChangeStatus(int id)
    {
        _categoryDal.ChangeStatus(id);
        _cache.Remove(CacheKeyAll);
    }
}