using Domain.Entities;
using Core.Application.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
﻿
using Core.Application.Repositories;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IAutomovilRepository : IRepository<Automovil>
    {
        Task<bool> AutomovilExistsAsync(string chasis);
        Task<bool> ExisteNumeroMotorAsync(string numeroMotor, int idActual);
        Task<Automovil> GetByChasisAsync(string numeroChasis);
    }
}