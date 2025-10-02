using Application.Repositories;
using Domain.Entities;
using Core.Infraestructure.Repositories.Sql; // Asumo BaseRepository está aquí
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infrastructure.Repositories.Sql
{
    // Asumo que StoreDbContext es tu contexto de datos y BaseRepository acepta DbContext.
    internal class AutomovilRepository(StoreDbContext context)
        : BaseRepository<Automovil>(context), IAutomovilRepository
    {
        // Implementación del Requisito 5 (GetByChasis)
        public async Task<Automovil> GetByChasisAsync(string chasis)
        {
            // Repository viene de BaseRepository, es el DbSet<Automovil>
            return await Repository.FirstOrDefaultAsync(a => a.NumeroChasis == chasis);
        }

        // Se requiere 'new' para ocultar la implementación de BaseRepository que devuelve List<T> 
        // y retornar IEnumerable<T> (Requisito 6).
        public new async Task<IEnumerable<Automovil>> FindAllAsync()
        {
            return await base.FindAllAsync();
        }
    }
}