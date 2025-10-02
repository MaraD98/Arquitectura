using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // Métodos de Lectura y Consulta
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> FindAllAsync();
        IQueryable<TEntity> Query();
        Task<long> CountAsync(Expression<Func<TEntity, bool>> filter);

        // Métodos de Escritura Asíncronos
        Task<object> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity); // Usado por UpdateAutomovilHandler
        Task DeleteAsync(TEntity entity); // Usado por DeleteAutomovilHandler
        Task SaveAsync(TEntity entity); // Usado para persistir cambios

        // Métodos Síncronos (Mantenidos por consistencia con tu BaseRepository original)
        object Add(TEntity entity);
        void Update(object id, TEntity entity);
        void Remove(params object[] keyValues);
        long Count(Expression<Func<TEntity, bool>> filter);
    }
}