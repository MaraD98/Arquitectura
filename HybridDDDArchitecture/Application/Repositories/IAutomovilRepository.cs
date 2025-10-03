using Domain.Entities;
using Core.Application.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Repositories
{
    public interface IAutomovilRepository : IRepository<Automovil>
    {
        Task<Automovil> GetByChasisAsync(string chasis);
        new Task<IEnumerable<Automovil>> FindAllAsync();
        Task<bool> AutomovilExistsAsync(string chasis);
        Task<bool> ExisteNumeroMotorAsync(string numeroMotor, int idActual);
        
    }
}