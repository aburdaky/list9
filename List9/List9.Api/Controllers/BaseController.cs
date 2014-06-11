using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Data;
using Data.Interfaces;
using System.Data.Entity.Infrastructure;
using Model.Interfaces;

namespace List9.Api.Controllers
{
    
    public abstract class BaseController<TEntity> : ApiController where TEntity : class, IHasId
    {
        public IUnitOfWork Uow;
        public IRepository<TEntity> Repo;
        //public RepsUKUser CurrentUser { get { return UserManager.FindById(User.Identity.GetUserId()); } }

        public BaseController()
            : this(UnitOfWork.Instantiate())
        {

        }

        public BaseController(IUnitOfWork uow)
        {
            Uow = uow;
            Repo = Uow.GetRepositoryForType<TEntity>();
          
        }

        // GET api/TEntity
        /// <summary>
        /// Api endpoint for entity queries.
        /// </summary>
        /// <returns>Collection of resulting entities.</returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return Repo.Get();
        }

        // GET api/TEntity/5
        /// <summary>
        /// Api endpoint for getting specific entities by its Id value.
        /// </summary>
        /// <param name="id">Id value of entity required.</param>
        /// <returns>Entity with Id value provided.</returns>
        public virtual IHttpActionResult GetById(int id)
        {
            TEntity entity = Repo.Find(id);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        // PUT api/TEntity/5
        /// <summary>
        /// Api endpoint for updating an entity.
        /// </summary>
        /// <param name="id">Id value of entity to be updated.</param>
        /// <param name="entity">Serialized representation of updated object.</param>
        /// <returns>Updated version of entity.</returns>
       
        public virtual IHttpActionResult Put(int id, TEntity entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }

            Repo.Update(entity);

            try
            {
                Uow.Commit(User.Identity.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok<TEntity>(entity);
        }

        // POST api/Base
        /// <summary>
        /// Api endpoint for creating a new entity.
        /// </summary>
        /// <param name="entity">Serialized representation of created object.</param>
        /// <returns>Created version of entity.</returns>
        
        public virtual IHttpActionResult Post(TEntity entity)
        {
            Repo.Add(entity);
            Uow.Commit(User.Identity.Name);

            return CreatedAtRoute("DefaultApi", new { id = entity.Id }, entity);
        }

        // DELETE api/Base/5
        /// <summary>
        /// Api endpoint for deleting a specific entity by its Id value.
        /// </summary>
        /// <param name="id">Id value of entity to be deleted.</param>
        /// <returns>Deleted entity.</returns>
        
        public virtual IHttpActionResult Delete(int id)
        {
            TEntity entity = Repo.Find(id);
            if (entity == null)
            {
                return NotFound();
            }

            Repo.Delete(entity);
            Uow.Commit(User.Identity.Name);

            return Ok(entity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Uow.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool EntityExists(int id)
        {
            return Repo.Exists(id);
        }
    }
}