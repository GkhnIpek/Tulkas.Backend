using Entities.Concrete;
using Infrastructure.Utilities.Results;
using System.Collections.Generic;
using Infrastructure.Entities.Concrete;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<OperationClaim>> GetClaims(User user);
        IDataResult<User> GetByMail(string email);
        IResult Add(User user);
    }
}
