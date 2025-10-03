using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // Métodos de Lectura y Consulta Asíncronos
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> FindAllAsync();

        Task<TEntity> FindOneAsync(params object[] keyValues);

        IQueryable<TEntity> Query();
        Task<long> CountAsync(Expression<Func<TEntity, bool>> filter);

        // Métodos de Escritura Asíncronos
        Task<object> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task SaveAsync(TEntity entity);

        // Métodos Síncronos (Mantenidos)
        object Add(TEntity entity);
        void Update(object id, TEntity entity);
        void Remove(params object[] keyValues);
        long Count(Expression<Func<TEntity, bool>> filter);
    }
}