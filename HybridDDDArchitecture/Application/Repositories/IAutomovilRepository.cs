using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
// using Core.Application.Repositories; // Mantener la herencia si existe, pero añadir los métodos que faltan en el Handler

namespace Application.Repositories
{
    // Asumiendo que SI hereda de IRepository, pero se añaden los métodos básicos para asegurar el compilador
    // Si no hereda de IRepository, la interfaz DEBE contener todos los métodos (GetById, Update, etc.)
    public interface IAutomovilRepository // Si hereda de IRepository<Automovil>
    {
        // Añadido para resolver CS1061 en UpdateAutomovilHandler.cs
        Task UpdateAsync(Automovil entity);

        // Añadido/Mantenido para asegurar que los Handlers puedan leer
        Task<Automovil> GetByIdAsync(int id);
        Task<Automovil> GetByChasisAsync(string chasis);
        Task<IEnumerable<Automovil>> FindAllAsync();
        Task AddAsync(Automovil entity);
        Task DeleteAsync(Automovil entity);
        Task<bool> AutomovilExistsAsync(string chasis);
    }
}