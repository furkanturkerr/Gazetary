using Business.Abstract;
using DataAccess.Abstarct;
using Entities.Concrate;

namespace Business.Concrate;

public class ImageManager : IImageService
{
    private readonly IImageDal _imageDal;

    public ImageManager(IImageDal imageDal)
    {
        _imageDal = imageDal;
    }

    public void Insert(Image t)
    {
        _imageDal.Insert(t);
    }

    public void Update(Image t)
    {
        _imageDal.Update(t);
    }

    public void Delete(Image t)
    {
        _imageDal.Delete(t);
    }

    public List<Image> GetAll()
    {
        return _imageDal.GetAll();
    }

    public Image GetById(int id)
    {
        return _imageDal.GetById(id);
    }
}