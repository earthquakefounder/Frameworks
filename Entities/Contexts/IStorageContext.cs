using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Contexts
{
    public interface IStorageContext<TEntity>
    {
        IQueryable<TEntity> EntityQuery { get; set; }
    }
}
