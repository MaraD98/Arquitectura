using Application.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Sql
{
    internal class AutomovilRepository : BaseRepository<Automovil>, IAutomovilRepository
    {
        private readonly StoreDbContext _context;

        public AutomovilRepository(StoreDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteNumeroMotorAsync(string numeroMotor, int idActual)
        {
            return await _context.Automoviles
                .AnyAsync(a => a.NumeroMotor == numeroMotor && a.Id != idActual);
        }
    }
}
