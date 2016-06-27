using Entities.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contexts
{
    public class StorageContext<TEntity> : BaseStorageContext<TEntity>
        where TEntity: class
    {
        public StorageContext(DatabaseConnections settings) : base(settings.Database) { }

        protected DbSet<TEntity> EntitySet { get { return Set<TEntity>(); } }

        public override IQueryable<TEntity> Entities { get { return EntitySet; } }

        public override TEntity Add(TEntity entity)
        {
            EntitySet.Add(entity);
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            EntitySet.Remove(entity);
        }
    }
}
