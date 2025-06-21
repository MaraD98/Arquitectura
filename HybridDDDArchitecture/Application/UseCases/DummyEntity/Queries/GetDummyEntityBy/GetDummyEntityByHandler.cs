using Application.DataTransferObjects;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;

namespace Application.UseCases.DummyEntity.Queries.GetDummyEntityBy
{
    internal sealed class GetDummyEntityByHandler(IDummyEntityRepository context) : IRequestQueryHandler<GetDummyEntityByQuery, DummyEntityDto>
    {
        private readonly IDummyEntityRepository _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<DummyEntityDto> Handle(GetDummyEntityByQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.DummyEntity entity = await _context.FindOneAsync(request.DummyIdProperty) ?? throw new EntityDoesNotExistException();
            return entity.To<DummyEntityDto>();
        }
    }
}
