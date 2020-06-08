using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Entities.Concrete;
using Infrastructure.Aspects.Autofac.Caching;
using Infrastructure.Aspects.Autofac.Logging;
using Infrastructure.Aspects.Autofac.Performance;
using Infrastructure.Aspects.Autofac.Transaction;
using Infrastructure.Aspects.Autofac.Validation;
using Infrastructure.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Infrastructure.DataAccess.Abstract;
using Infrastructure.Utilities.Business;
using Infrastructure.Utilities.Results;

namespace Business.Concrete
{
    public partial class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        public ProductManager(IUnitOfWork unitOfWork, ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
        }

        public IDataResult<Product> Get(int productId)
        {
            var product = _unitOfWork.GetRepository<Product>().Get(p => p.ProductId == productId);
            return new SuccessDataResult<Product>(product);
        }

        [PerformanceAspect(5)]
        public IDataResult<List<Product>> GetList()
        {
            Thread.Sleep(5000); //Performans testi için yazıldı.
            var productList = _unitOfWork.GetRepository<Product>().GetList().ToList();
            return new SuccessDataResult<List<Product>>(productList);
        }

        //[SecuredOperation("Product.List,Admin")]
        [LogAspect(typeof(FileLogger), Priority = 1)]
        [LogAspect(typeof(DatabaseLogger), Priority = 2)]
        [CacheAspect(duration: 10, Priority = 3)]
        public IDataResult<List<Product>> GetListByCategory(int categoryId)
        {            
            var productList = _unitOfWork.GetRepository<Product>().GetList(p => p.CategoryId == categoryId).ToList();
            return new SuccessDataResult<List<Product>>(productList);
        }

        [ValidationAspect(typeof(ProductValidator), Priority = 1)]
        [CacheRemoveAspect("IProductService.Get", Priority = 2)]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                CheckIfCategoryIsEnabled());

            if (result != null)
            {
                return result;
            }
            _unitOfWork.GetRepository<Product>().Add(product);
            _unitOfWork.SaveChanges();
            return new SuccessResult(Messages.ProductAdded);
        }

        public IResult Delete(Product product)
        {
            _unitOfWork.GetRepository<Product>().Delete(product);
            _unitOfWork.SaveChanges();
            return new SuccessResult(Messages.ProductDeleted);
        }

        public IResult Update(Product product)
        {
            _unitOfWork.GetRepository<Product>().Update(product);
            _unitOfWork.SaveChanges();
            return new SuccessResult(Messages.ProductUpdated);
        }

        [TransactionScopeAspect] //Transaction işlemini Unitofwork patterni zaten ilgilenmektedir.Bu ekstra bir özellik olarak kontrol etmektedir.
        public IResult TransactionalOperationTest(Product product)
        {
            _unitOfWork.GetRepository<Product>().Update(product);
            _unitOfWork.GetRepository<Product>().Add(product);
            _unitOfWork.SaveChanges();
            return new SuccessResult(Messages.ProductUpdated);
        }
    }
}
