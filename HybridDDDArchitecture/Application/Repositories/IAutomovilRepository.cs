using Core.Application.Repositories;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IAutomovilRepository : IRepository<Automovil>
    {
        Task<bool> ExisteNumeroMotorAsync(string numeroMotor, int idActual);

        new Task<Automovil> GetByIdAsync(int id); // va sin new pero me tira un error y me pide que lo oculte

        Task<Automovil> GetByChasisAsync(string numeroChasis);
    }

}
