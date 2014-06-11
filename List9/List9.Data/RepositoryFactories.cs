﻿using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Data
{
    public class RepositoryFactories : IRepositoryFactories
    {
        private readonly IDictionary<Type, Func<DbContext, object>> _repositoryFactories;

        public RepositoryFactories()
        {
            _repositoryFactories = GetCustomFactories();
        }

        public RepositoryFactories(IDictionary<Type, Func<DbContext, object>> factories)
        {
            _repositoryFactories = factories;
        }

        private IDictionary<Type, Func<DbContext, object>> GetCustomFactories()
        {
            var overrides = new Dictionary<Type, Func<DbContext, object>>();
            return overrides;
        }

        public Func<DbContext, object> GetRepositoryFactory<TEntity>()
        {
            Func<DbContext, object> factory;
            _repositoryFactories.TryGetValue(typeof(TEntity), out factory);
            return factory;
        }

        public Func<DbContext, object> GetRepositoryFactoryForEntityType<TEntity>() where TEntity : class
        {
            return GetRepositoryFactory<TEntity>() ?? DefaultEntityRepositoryFactory<TEntity>();
        }

        protected virtual Func<DbContext, object> DefaultEntityRepositoryFactory<TEntity>() where TEntity : class
        {
            return dbContext => new List9.Data.Repositories.Repository <TEntity>(dbContext);
        }
    }
}
