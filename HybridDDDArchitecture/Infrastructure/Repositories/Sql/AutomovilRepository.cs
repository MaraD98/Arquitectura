using Application.Repositories;
using Domain.Entities;
using Core.Infraestructure.Repositories.Sql; // Namespace de BaseRepository
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Core.Application.Repositories; // Para IRepository
using Infrastructure.Repositories.Sql; // Para StoreDbContext (asumido)

namespace Infrastructure.Repositories.Sql
{
    // Hereda de BaseRepository y le pasa el contexto.
    internal class AutomovilRepository(StoreDbContext context)
        : BaseRepository<Automovil>(context), IAutomovilRepository
    {
        // Método específico requerido por IAutomovilRepository (GetByChasis)
        public async Task<Automovil> GetByChasisAsync(string chasis)
        {
            // Usamos Query() de BaseRepository para construir la consulta
            return await Query().FirstOrDefaultAsync(a => a.NumeroChasis == chasis);
        }

        // Métodos de IRepository que ya están cubiertos por BaseRepository, 
        // a menos que sus firmas NO coincidan, en cuyo caso necesitas:

        // 🚨 Si IAutomovilRepository requiere Task<IEnumerable<Automovil>> y BaseRepository 
        // devuelve Task<List<Automovil>>, necesitas una nueva implementación (como la que tenías).
        public new async Task<IEnumerable<Automovil>> FindAllAsync()
        {
            // Llama al método base que devuelve Task<List<Automovil>> y lo cast a IEnumerable
            return await base.FindAllAsync();
        }
    }
}