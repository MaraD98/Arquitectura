using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // CRUD (Se mantiene la firma de tu código para evitar errores en BaseRepository)
        object Add(TEntity entity);
        Task<object> AddAsync(TEntity entity); // Tu BaseRepository lo usa
        Task DeleteAsync(TEntity entity);
        void Remove(params object[] keyValues);
        void Update(object id, TEntity entity);

        // Métodos requeridos por los Handlers
        Task<TEntity> GetByIdAsync(int id);
        Task UpdateAsync(TEntity entity); // Requerido por UpdateAutomovilHandler

        // Consultas y Utilidades
        IQueryable<TEntity> Query();
        Task<long> CountAsync(Expression<Func<TEntity, bool>> filter);
        Task<List<TEntity>> FindAllAsync();

        // Se asume que este método SaveAsync realiza la acción de persistencia de la UoW o del Contexto.
        Task SaveAsync(TEntity entity);
    }
}