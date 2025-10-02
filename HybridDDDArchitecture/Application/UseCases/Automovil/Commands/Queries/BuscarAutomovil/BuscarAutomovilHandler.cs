using Application.DataTransferObjects;
using Application.Repositories;
using Core.Application;

namespace Application.UseCases.Automovil.Queries.BuscarAutomovil
{
    internal class BuscarAutomovilHandler : IRequestQueryHandler<BuscarAutomovilQuery, QueryResult<AutomovilDto>>
    {
        private readonly IAutomovilRepository _AutomovilRepository;
        public BuscarAutomovilHandler(IAutomovilRepository _automovilRepository)
        (
            _automovilRepository = automovilRepository?? throw new ArgumentNullException(nameof(automovilRepository));
        )

        public async Task<QueryResult<AutomovilDto>> Handle(BuscarAutomovilQuery request, CancellationToken cancellationToken)
        {
              IList<Domain.Entities.Automovil> entities = await _automovilRepository.FindAllAsync();

            return new QueryResult<DummyEntityDto>(entities.AutomovilDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}