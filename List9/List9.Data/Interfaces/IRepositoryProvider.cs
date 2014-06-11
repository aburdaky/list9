using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepositoryProvider
    {
        DbContext Context { get; set; }


        IRepository<TEntity> GetRepositoryForType<TEntity>() where TEntity : class;
        TEntity GetRepository<TEntity>(Func<DbContext, object> factory = null) where TEntity : class;
        void SetRepository<TEntity>(TEntity repository);

    }
}
