using Core.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Infraestructure.Repositories.Sql
{
    // 🚨 CORRECCIÓN IDE0290: Uso del constructor principal
    public abstract class BaseRepository<TEntity>(DbContext context) : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext Context = context;
        protected readonly DbSet<TEntity> Repository = context.Set<TEntity>();

        // --- Implementaciones Asíncronas ---
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await Repository.FindAsync(id);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            Repository.Update(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            Repository.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task<List<TEntity>> FindAllAsync()
        {
            return await Repository.ToListAsync();
        }

        public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Repository.LongCountAsync(filter);
        }

        public virtual async Task<object> AddAsync(TEntity entity)
        {
            await Repository.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task SaveAsync(TEntity entity)
        {
            await Context.SaveChangesAsync();
        }

        public virtual async Task<TEntity> FindOneAsync(params object[] keyValues)
        {
            return await Repository.FindAsync(keyValues);
        }

        // --- Implementaciones Síncronas ---
        public virtual object Add(TEntity entity)
        {
            Repository.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        // 🚨 CORRECCIÓN IDE0305: Simplificación de inicialización de colección
        public virtual List<TEntity> FindAll()
        {
            return Repository.ToList();
        }

        public virtual TEntity FindOne(params object[] keyValues)
        {
            return Repository.Find(keyValues);
        }

        public virtual void Remove(params object[] keyValues)
        {
            var entity = Repository.Find(keyValues);
            // 🚨 CORRECCIÓN IDE0270: Simplificación de la comprobación
            if (entity is not null)
            {
                Repository.Remove(entity);
                Context.SaveChanges();
            }
        }

        public virtual void Update(object id, TEntity entity)
        {
            Repository.Update(entity);
            Context.SaveChanges();
        }

        public virtual long Count(Expression<Func<TEntity, bool>> filter)
        {
            return Repository.LongCount(filter);
        }

        public virtual IQueryable<TEntity> Query()
        {
            return Repository.AsQueryable();
        }
    }
}