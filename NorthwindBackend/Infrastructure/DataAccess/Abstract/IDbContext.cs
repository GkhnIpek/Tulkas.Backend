using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Abstract
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class, IEntity;
        int SaveChanges();
        void Dispose();
    }
}
