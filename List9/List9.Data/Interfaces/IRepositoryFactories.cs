using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepositoryFactories
    {
        Func<System.Data.Entity.DbContext, object> GetRepositoryFactory<TEntity>();
        Func<System.Data.Entity.DbContext, object> GetRepositoryFactoryForEntityType<TEntity>() where TEntity : class;
    }
}
