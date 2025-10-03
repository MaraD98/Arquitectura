using Application.Repositories;
using Domain.Entities;
using Core.Infraestructure.Repositories.Sql;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Infrastructure.Repositories.Sql; 
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql
{
    internal class AutomovilRepository(StoreDbContext context)
        : BaseRepository<Automovil>(context), IAutomovilRepository
    {
        // 🚨 Implementación requerida por IAutomovilRepository
        public async Task<bool> AutomovilExistsAsync(string chasis)
        {
            return await Repository.AnyAsync(a => a.NumeroChasis == chasis);
        }

        public new async Task<IEnumerable<Automovil>> FindAllAsync()
        {
            return await base.FindAllAsync();
        }

        public async Task<bool> ExisteNumeroMotorAsync(string numeroMotor, int idActual)
        {
            return await Repository
                .AnyAsync(a => a.NumeroMotor == numeroMotor && a.Id != idActual);
        }

        public async Task<Automovil> GetByChasisAsync(string numeroChasis)
        {
            if (string.IsNullOrEmpty(numeroChasis)) return null;

            return await Repository.FirstOrDefaultAsync(a => a.NumeroChasis == numeroChasis);
        }
    }
}