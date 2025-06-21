using Core.Application.Repositories;
using Domain.Entities;

namespace Application.Repositories
{
    /// <summary>
    /// Ejemplo de interface de un repositorio de entidad Dummy
    /// Todo repositorio debe implementar la interfaz <see cref="IRepository{TEntity}"/>
    /// donde <c TEntity> es la entidad de dominio que queremos persistir
    /// </summary>
    public interface IDummyEntityRepository : IRepository<DummyEntity>
    {
        //Aqui se definen propiedades y metodos Custom.
    }
}
