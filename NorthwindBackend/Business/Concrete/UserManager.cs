using Business.Abstract;
using Business.Constants;
using DataAccess.Abstract;
using Infrastructure.DataAccess.Abstract;
using Infrastructure.Entities.Concrete;
using Infrastructure.Utilities.Results;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IUserDal userDal, IUnitOfWork unitOfWork)
        {
            _userDal = userDal;
            _unitOfWork = unitOfWork;
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            var operationClaims = _userDal.GetClaims(user);
            return new SuccessDataResult<List<OperationClaim>>(operationClaims);
        }

        public IDataResult<User> GetByMail(string email)
        {
            var user = _unitOfWork.GetRepository<User>().Get(p => p.Email == email);
            return new SuccessDataResult<User>(user);
        }

        public IResult Add(User user)
        {
            _unitOfWork.GetRepository<User>().Add(user);
            _unitOfWork.SaveChanges();
            return new SuccessResult(Messages.UserAdded);
        }
    }
}
