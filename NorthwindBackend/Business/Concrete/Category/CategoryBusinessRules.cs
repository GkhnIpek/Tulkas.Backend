using Business.Constants;
using Entities.Concrete;
using Infrastructure.Utilities.Results;

namespace Business.Concrete
{
    public partial class CategoryManager
    {
        private IResult CheckIfCategoryNameExists(string categoryName)
        {
            var category = _unitOfWork.GetRepository<Category>().Get(p => p.CategoryName == categoryName);
            if (category != null)
            {
                return new ErrorResult(Messages.CategoryNameAlreadyExists);
            }

            return new SuccessResult();
        }
    }
}
