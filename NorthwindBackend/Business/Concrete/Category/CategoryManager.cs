using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using Business.Constants;
using Entities.Concrete;
using Infrastructure.DataAccess.Abstract;
using Infrastructure.Utilities.Business;
using Infrastructure.Utilities.Results;

namespace Business.Concrete
{
    public partial class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IDataResult<Category> Get(int categoryId)
        {
            var category = _unitOfWork.GetRepository<Category>().Get(p => p.CategoryId == categoryId);
            return new SuccessDataResult<Category>(category);
        }

        public IDataResult<List<Category>> GetList()
        {
            var categories = _unitOfWork.GetRepository<Category>().GetList().ToList();
            return new SuccessDataResult<List<Category>>(categories);
        }

        public IResult Add(Category category)
        {
            IResult result = BusinessRules.Run(CheckIfCategoryNameExists(category.CategoryName));
            if (result != null)
            {
                return result;
            }
            
            _unitOfWork.GetRepository<Category>().Add(category);
            _unitOfWork.SaveChanges();
            return new SuccessResult(Messages.CategoryAdded);
        }

        public IResult Delete(Category category)
        {
            _unitOfWork.GetRepository<Category>().Delete(category);
            _unitOfWork.SaveChanges();
            return new SuccessResult(Messages.CategoryDeleted);
        }

        public IResult Update(Category category)
        {
            _unitOfWork.GetRepository<Category>().Update(category);
            _unitOfWork.SaveChanges();
            return new SuccessResult(Messages.CategoryUpdated);
        }
    }
}
