using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contexts
{
    public abstract class BaseStorageContext : DbContext
    {
        public BaseStorageContext(string connectionString)
            : base(connectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    public abstract class BaseStorageContext<TEntity> : BaseStorageContext, IStorageContext<TEntity>
    {
        public BaseStorageContext(string connectionString)
            : base(connectionString)
        {
            MethodInfo initializer = typeof(Database).GetMethod("SetInitializer");

            MethodInfo contextInitializer = initializer.MakeGenericMethod(this.GetType());

            contextInitializer.Invoke(null, new object[] { null });
        }

        public abstract IQueryable<TEntity> Entities { get; }

        public abstract TEntity Add(TEntity entity);
        public abstract void Delete(TEntity entity);
    }
}
