using Application.Repositories;
using Domain.Entities;
using Core.Infraestructure.Repositories.Sql; // Namespace de BaseRepository
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic; // Para IEnumerable<Automovil>

// CORRECCIÓN CS0234: Agregamos el using para el DbContext
using Infrastructure.Repositories.Sql;

namespace Infrastructure.Repositories.Sql
{
    // El constructor usa la sintaxis de constructor primario de C# (net8.0)
    internal class AutomovilRepository(StoreDbContext context) : BaseRepository<Automovil>(context), IAutomovilRepository
    {
        // CORRECCIÓN CS7036: Usamos 'BaseRepository<Automovil>(context)' para pasar el argumento requerido.
        // El constructor primario elimina la necesidad de la variable _context si no se usa fuera del constructor base.

        // 1. MÉTODOS ESPECÍFICOS (mantienen las llamadas a la base Query())
        public async Task<Automovil> GetByChasisAsync(string chasis)
        {
            // Query() viene de BaseRepository
            return await Query().FirstOrDefaultAsync(a => a.NumeroChasis == chasis);
        }

        public async Task<bool> AutomovilExistsAsync(string chasis)
        {
            return await Query().AnyAsync(a => a.NumeroChasis == chasis);
        }

        // 2. CORRECCIÓN CS0535/CS0738: Implementar los métodos de IAutomovilRepository
        // cuyos tipos de retorno no coinciden con BaseRepository o que no están en BaseRepository.

        // UpdateAsync (CS0535) - Implementación requerida por IAutomovilRepository
        // BaseRepository tiene SaveAsync, que probablemente hace lo mismo.
        public Task UpdateAsync(Automovil entity)
        {
            // Usamos el método SaveAsync de BaseRepository (o el método correcto para actualizar)
            return SaveAsync(entity);
        }

        // FindAllAsync (CS0738) - El tipo de retorno de BaseRepository es List<TEntity> o Task<List<TEntity>> 
        // pero IAutomovilRepository requiere Task<IEnumerable<Automovil>>.
        public new async Task<IEnumerable<Automovil>> FindAllAsync()
        {
            // Llamamos al método base y convertimos el tipo de retorno.
            return await base.FindAllAsync();
        }

        // AddAsync (CS0738) - El tipo de retorno de BaseRepository es Task<object> 
        // pero IAutomovilRepository requiere Task (void).
        public new async Task AddAsync(Automovil entity)
        {
            // Ejecutamos el método base que devuelve Task<object> y descartamos el objeto.
            await base.AddAsync(entity);
        }

        // GetByIdAsync y DeleteAsync deben coincidir automáticamente o ser implementados
        // si sus firmas no coinciden con las de BaseRepository.
    }
}