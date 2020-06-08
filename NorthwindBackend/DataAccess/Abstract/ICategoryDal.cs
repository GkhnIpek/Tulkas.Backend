using Entities.Concrete;
using Infrastructure.DataAccess.Abstract;

namespace DataAccess.Abstract
{
    public interface ICategoryDal : IEntityRepository<Category>
    {

    }
}
