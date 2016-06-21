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

        public virtual IDbSet<TEntity> DbSet { get; set; }

        public virtual IQueryable<TEntity> Entities { get { return DbSet; } }

        public TEntity Add(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }
    }
}
