using Application.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql
{
    internal class AutomovilRepository : BaseRepository<Automovil>, IAutomovilRepository
    {
        public AutomovilRepository(StoreDbContext context) : base(context)
        {
        }

        public async Task<bool> ExisteNumeroMotorAsync(string numeroMotor, int idActual)
        {
            return await Repository
                .AnyAsync(a => a.NumeroMotor == numeroMotor && a.Id != idActual);
        }

        public async Task<Automovil> GetByChasisAsync(string numeroChasis)
        {
            if (string.IsNullOrEmpty(numeroChasis)) return null;

            return await Repository
                .FirstOrDefaultAsync(a => a.NumeroChasis == numeroChasis);
        }
    }
}