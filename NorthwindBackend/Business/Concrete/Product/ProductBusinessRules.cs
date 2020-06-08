using Business.Constants;
using Entities.Concrete;
using Infrastructure.Utilities.Results;

namespace Business.Concrete
{
    public partial class ProductManager
    {
        private IResult CheckIfCategoryIsEnabled()
        {
            var result = _categoryService.GetList();
            if (result.Data.Count > 10)
            {
                return new ErrorResult(Messages.CategoryIsEnabled);
            }

            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var product = _unitOfWork.GetRepository<Product>().Get(p => p.ProductName == productName);
            if (product != null)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }

            return new SuccessResult();
        }
    }
}
