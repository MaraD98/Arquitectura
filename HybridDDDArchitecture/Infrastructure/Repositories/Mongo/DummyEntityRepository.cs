using Application.Repositories;
using Core.Infraestructure.Repositories.MongoDb;
using Domain.Entities;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Mongo
{
    /// <summary>
    /// Ejemplo de repositorio Mongo de entidad Dummy
    /// Todo repositorio debe implementar la interfaz que hereda de <see cref="Common.Application.Repositories.MongoDb.IRepository{T}"/>
    /// creada en la capa de aplicacion, y heredar de <see cref="BaseRepository{T}"/>
    /// donde <c T> es la entidad de dominio que queremos persistir
    /// </summary>
    // 🚨 CORRECCIÓN IDE0290: Uso de constructor principal
    internal sealed class DummyEntityRepository(IMongoDatabase database)
        : BaseRepository<DummyEntity>(database, "DummyEntities"), IDummyEntityRepository
    {
        // El cuerpo del constructor ya no es necesario
    }
}