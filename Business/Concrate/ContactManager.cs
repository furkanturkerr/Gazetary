using Business.Abstract;
using DataAccess.Abstarct;
using Entities.Concrate;

namespace Business.Concrate;

public class ContactManager : IContactService
{
    private readonly IContactDal _contactDal;

    public ContactManager(IContactDal contactDal)
    {
        _contactDal = contactDal;
    }

    public async void Insert(Contact t)
    {
        _contactDal.Insert(t);
    }

    public void Update(Contact t)
    {
        _contactDal.Update(t);
    }

    public void Delete(Contact t)
    {
       _contactDal.Delete(t);
    }

    public List<Contact> GetAll()
    {
        return _contactDal.GetAll();
    }

    public Contact GetById(int id)
    {
        return _contactDal.GetById(id);
    }
}