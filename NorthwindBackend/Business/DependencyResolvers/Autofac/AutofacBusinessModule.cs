using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Infrastructure.DataAccess.Abstract;
using Infrastructure.DataAccess.Concrete.EntityFramework;
using Infrastructure.Utilities.Interceptors;
using Infrastructure.Utilities.Security.Jwt;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region Services
            builder.RegisterType<ProductManager>().As<IProductService>();
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<AuthManager>().As<IAuthService>();
            #endregion

            #region Contexts - DAL
            builder.RegisterType<EfUnitOfWork<NorthwindContext>>().As<IUnitOfWork>();
            builder.RegisterType<EfProductDal>().As<IProductDal>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();
            #endregion

            #region Utilities
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();
            #endregion

            #region Interception
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().EnableInterfaceInterceptors(
                new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
            #endregion
        }
    }
}
