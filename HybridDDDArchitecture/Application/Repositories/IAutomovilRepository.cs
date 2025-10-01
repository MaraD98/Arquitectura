// File: Application/Repositories/IAutomovilRepository.cs

using Core.Application.Repositories;
using Domain.Entities;

namespace Application.Repositories
{
    // The IAutomovilRepository will automatically inherit all methods from IRepository<Automovil>
    public interface IAutomovilRepository : IRepository<Automovil>
    {
        Task<Automovil> FindOneByChasisAsync(string chasis);
    }
}