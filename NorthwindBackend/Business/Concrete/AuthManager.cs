using Business.Abstract;
using Business.Constants;
using Entities.Dtos;
using Infrastructure.DataAccess.Abstract;
using Infrastructure.Entities.Concrete;
using Infrastructure.Utilities.Results;
using Infrastructure.Utilities.Security.Hashing;
using Infrastructure.Utilities.Security.Jwt;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthManager(ITokenHelper tokenHelper, IUserService userService, IUnitOfWork unitOfWork)
        {
            _tokenHelper = tokenHelper;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            var userExistCheck = UserExists(userForRegisterDto.Email);
            if (userExistCheck.Success)
            {
                HashingHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
                var user = new User
                {
                    Email = userForRegisterDto.Email,
                    FirstName = userForRegisterDto.FirstName,
                    LastName = userForRegisterDto.LastName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true
                };

                _unitOfWork.GetRepository<User>().Add(user);
                _unitOfWork.SaveChanges();
                return new SuccessDataResult<User>(user, Messages.UserRegistered);
            }

            return new ErrorDataResult<User>(message: userExistCheck.Message);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (!userToCheck.Success)
            {
                return new ErrorDataResult<User>(message:userToCheck.Message);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck.Data, Messages.SuccessfulLogin);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email).Data != null)
            {
                return new ErrorResult(Messages.UserExists);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            if (!claims.Success)
            {
                return new ErrorDataResult<AccessToken>(message: claims.Message);
            }
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }
    }
}
