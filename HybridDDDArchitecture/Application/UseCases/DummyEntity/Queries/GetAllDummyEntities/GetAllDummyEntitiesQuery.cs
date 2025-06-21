using Application.DataTransferObjects;
using Core.Application;

namespace Application.UseCases.DummyEntity.Queries.GetAllDummyEntities
{
    public class GetAllDummyEntitiesQuery : QueryRequest<QueryResult<DummyEntityDto>>
    {
    }
}
