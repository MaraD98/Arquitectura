using Core.Application.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Infraestructure.Repositories.MongoDb
{
    // Asumimos que la inyección necesita el objeto de base de datos y el nombre de la colección
    public abstract class BaseRepository<TEntity>(IMongoDatabase database, string collectionName) : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly IMongoCollection<TEntity> Repository = database.GetCollection<TEntity>(collectionName);
        private readonly FilterDefinitionBuilder<TEntity> _filterBuilder = Builders<TEntity>.Filter;

        // --- Implementaciones Asíncronas ---

        // Asumiendo que TEntity tiene un campo 'Id' (o similar) que se usa como clave
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            var filter = _filterBuilder.Eq("Id", id);
            return await Repository.Find(filter).FirstOrDefaultAsync();
        }

        // 🚨 CORRECCIÓN CS0535: Implementación de UpdateAsync
        public virtual async Task UpdateAsync(TEntity entity)
        {
            // Se asume que la entidad tiene una propiedad 'Id'
            var idValue = entity.GetType().GetProperty("Id")?.GetValue(entity);
            if (idValue == null) throw new InvalidOperationException("La entidad debe tener una propiedad 'Id' para usar UpdateAsync.");

            var filter = _filterBuilder.Eq("Id", idValue);
            await Repository.ReplaceOneAsync(filter, entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            var idValue = entity.GetType().GetProperty("Id")?.GetValue(entity);
            if (idValue == null) return;
            var filter = _filterBuilder.Eq("Id", idValue);
            await Repository.DeleteOneAsync(filter);
        }

        public virtual async Task<List<TEntity>> FindAllAsync()
        {
            return await Repository.Find(_filterBuilder.Empty).ToListAsync();
        }

        public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Repository.CountDocumentsAsync(filter);
        }

        public virtual async Task<object> AddAsync(TEntity entity)
        {
            await Repository.InsertOneAsync(entity);
            return entity;
        }

        public virtual Task SaveAsync(TEntity entity)
        {
            // En MongoDb, las operaciones ya persisten los cambios.
            return Task.CompletedTask;
        }

        public virtual async Task<TEntity> FindOneAsync(params object[] keyValues)
        {
            // En MongoDB, 'keyValues' es complejo, asumimos que es el Id y delegamos
            if (keyValues.Length == 1 && keyValues[0] is int id)
            {
                return await GetByIdAsync(id);
            }
            return null;
        }


        // --- Implementaciones Síncronas (Para cumplir con IRepository) ---
        public virtual object Add(TEntity entity)
        {
            Repository.InsertOne(entity);
            return entity;
        }

        public virtual void Update(object id, TEntity entity)
        {
            var filter = _filterBuilder.Eq("Id", id);
            Repository.ReplaceOne(filter, entity);
        }

        public virtual void Remove(params object[] keyValues)
        {
            if (keyValues.Length == 1)
            {
                var filter = _filterBuilder.Eq("Id", keyValues[0]);
                Repository.DeleteOne(filter);
            }
        }

        public virtual long Count(Expression<Func<TEntity, bool>> filter)
        {
            return Repository.CountDocuments(filter);
        }

        public virtual IQueryable<TEntity> Query()
        {
            return Repository.AsQueryable();
        }
    }
}