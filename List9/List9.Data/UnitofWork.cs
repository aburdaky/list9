using Data.Interfaces;
using Model.Interfaces;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; private set; }
        private IRepositoryProvider provider { get; set; }
        public UnitOfWork(IRepositoryProvider repositoryProvider)
        {
            Context = CreateContext();
            provider = ConfigureRepositoryProvider(repositoryProvider);
        }

        private DbContext CreateContext()
        {
            var context = new List9Context();
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.ProxyCreationEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;
            return context;
        }

        private IRepositoryProvider ConfigureRepositoryProvider(IRepositoryProvider repositoryProvider)
        {
            repositoryProvider.Context = Context;
            return repositoryProvider;
        }

        public void Commit(string username = null)
        {
            Context.ChangeTracker.DetectChanges();

            List<DbEntityEntry> created = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            foreach (var entry in created)
            {
                if (entry.Entity is IAudited)
                {
                    IAudited auditable = entry.Entity as IAudited;
                    auditable.CreatedBy = username ?? "";
                    auditable.CreationDate = DateTime.Now;
                }
            }

            List<DbEntityEntry> modified = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            foreach (var entry in modified)
            {
                if (entry.Entity is IAudited)
                {
                    IAudited auditable = entry.Entity as IAudited;
                    auditable.LastEditedBy = username ?? "";
                    auditable.LastEditDate = DateTime.Now;
                }
            }
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Context != null)
                {
                    Context.Dispose();
                }
            }
        }

        #region IUnitOfWork Members

        public IRepository<Project>Projects { get { return provider.GetRepositoryForType<Project>(); } }
        public IRepository<Model.Models.Task> Tasks { get { return provider.GetRepositoryForType<Model.Models.Task>(); } }

        public IRepository<TaskCategory> TaskCategories { get { return provider.GetRepositoryForType<TaskCategory>(); } }
        //public IRepository<User> Users { get { return provider.GetRepositoryForType<User>(); } }

        

        public IRepository<TEntity> GetRepositoryForType<TEntity>() where TEntity : class
        {
            return provider.GetRepositoryForType<TEntity>();
        }

        #endregion

        public static UnitOfWork Instantiate()
        {
            return new UnitOfWork(new RepositoryProvider(new RepositoryFactories()));
        }
    }
}
