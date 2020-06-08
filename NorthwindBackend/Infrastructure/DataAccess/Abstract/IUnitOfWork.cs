using Infrastructure.Entities;
using System;

namespace Infrastructure.DataAccess.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IEntityRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IEntity, new();

        int SaveChanges();
    }
}
