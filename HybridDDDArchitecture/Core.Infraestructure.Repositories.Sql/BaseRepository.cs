using Core.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Infraestructure.Repositories.Sql
{
    // Usamos el TContext genérico que requiere tu código anterior
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> Repository;

        protected BaseRepository(DbContext context)
        {
            Context = context;
            Repository = context.Set<TEntity>();
        }

        // --- Implementaciones Asíncronas (Requeridas por Handlers) ---
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            // Busca por clave primaria (válido si TEntity tiene una propiedad 'Id')
            return await Repository.FindAsync(id);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            Repository.Update(entity);
            await SaveAsync(entity); // Persiste el cambio
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
            return entity; // Devuelve el objeto, según tu interfaz
        }

        public virtual async Task SaveAsync(TEntity entity)
        {
            await Context.SaveChangesAsync();
        }

        // --- Implementaciones Síncronas (Para cumplir con IRepository) ---
        public virtual object Add(TEntity entity)
        {
            Repository.Add(entity);
            Context.SaveChanges();
            return entity;
        }

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
            if (entity != null)
            {
                Repository.Remove(entity);
                Context.SaveChanges();
            }
        }

        public virtual void Update(object id, TEntity entity)
        {
            // Asume que la entidad ya está en el contexto o la adjunta
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

        // Implementación requerida por IRepository que apunta a SaveAsync
        public virtual async Task<TEntity> FindOneAsync(params object[] keyValues)
        {
            return await Repository.FindAsync(keyValues);
        }
    }
}