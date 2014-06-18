using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit(string username = null);

        DbContext Context { get; }

        IRepository<Project> Projects { get; }
        IRepository<Model.Models.Task> Tasks { get; }
        IRepository<TaskCategory> TaskCategories { get;}
        //IRepository<User> Users { get; }

        IRepository<TEntity> GetRepositoryForType<TEntity>() where TEntity : class;

    }
}
