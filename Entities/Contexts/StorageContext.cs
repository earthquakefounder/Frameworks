using Entities.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contexts
{
    public class StorageContext<TEntity> : DbContext, IStorageContext<TEntity>
        where TEntity: class
    {
        public StorageContext(DatabaseConnections settings) : base(settings.Database) { }

        protected DbSet<TEntity> EntitySet { get { return Set<TEntity>(); } }

        public virtual IQueryable<TEntity> Entities { get { return EntitySet; } }

        public virtual TEntity Add(TEntity entity)
        {
            EntitySet.Add(entity);
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            EntitySet.Remove(entity);
        }
    }
}
