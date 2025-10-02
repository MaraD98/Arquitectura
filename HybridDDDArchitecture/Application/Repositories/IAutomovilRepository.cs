using Domain.Entities;
using Core.Application.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Repositories
{
    // Hereda los métodos base del IRepository
    public interface IAutomovilRepository : IRepository<Automovil>
    {
        // Método específico para el requisito GetByChasis
        Task<Automovil> GetByChasisAsync(string chasis);

        // 🚨 Si tus queries en el controller requieren IEnumerable, debes sobreescribir la firma aquí.
        new Task<IEnumerable<Automovil>> FindAllAsync();
    }
}