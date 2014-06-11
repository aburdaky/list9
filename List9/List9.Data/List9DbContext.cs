using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class List9Context: DbContext
    {
        public List9Context()
            : base("List9DbConnectionString")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<List9Context>()); 
            Configuration.AutoDetectChangesEnabled = false;
Configuration.LazyLoadingEnabled=false;
    Configuration.ProxyCreationEnabled=false;
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Model.Models.Task> Tasks { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

