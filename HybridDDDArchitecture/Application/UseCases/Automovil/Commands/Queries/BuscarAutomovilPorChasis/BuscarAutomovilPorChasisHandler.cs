using Application.DataTransferObjects;
using Application.Repositories;
using Core.Application;

namespace Application.UseCases.Automovil.Queries.BuscarAutomovilPorChasis 
{
    public class BuscarAutomovilPorChasisHandler : IRequestQueryHandler<BuscarAutomovilByChasisQuery, QueryResult<AutomovilByChasisDto>>
    {
        private readonly IAutomovilRepository _automovilRepository;
        public BuscarAutomovilPorChasisHandler(IAutomovilRepository _automovilRepository)
        {
            this._automovilRepository = _automovilRepository;
        }
        public async Task<QueryResult<AutomovilByChasisDto>> Handle(BuscarAutomovilByChasisQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.Automovil entity = await _automovilRepository.FindOneByChasisAsync(request.Chasis) ?? throw new EntityDoesNotExistException();
            return entity.To<AutomovilDto>();
        }
    }
}