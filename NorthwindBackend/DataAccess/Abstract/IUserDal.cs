using Entities.Concrete;
using Infrastructure.DataAccess.Abstract;
using System.Collections.Generic;
using Infrastructure.Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user);
    }
}
