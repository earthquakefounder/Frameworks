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

        public virtual IDbSet<TEntity> Entities { get; set; }

        public virtual IQueryable<TEntity> EntityQuery { get; set; }
    }
}
