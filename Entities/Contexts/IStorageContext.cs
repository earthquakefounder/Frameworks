using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contexts
{
    public interface IStorageContext<TEntity>
    {
        IQueryable<TEntity> Entities { get; }

        TEntity Add(TEntity entity);

        void Delete(TEntity entity);

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
