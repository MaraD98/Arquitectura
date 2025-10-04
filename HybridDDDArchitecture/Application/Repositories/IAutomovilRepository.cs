using Core.Application.Repositories;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IAutomovilRepository : IRepository<Automovil>
    {
        Task<bool> ExisteNumeroMotorAsync(string numeroMotor, int idActual);
        Task<Automovil> GetByChasisAsync(string numeroChasis);
    }

}
