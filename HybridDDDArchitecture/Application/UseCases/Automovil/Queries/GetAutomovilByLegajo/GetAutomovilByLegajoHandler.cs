using Application.DataTransferObjects;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;


namespace Application.UseCases.Automovil.Queries.GetAutomovilByLegajo
{
    internal class GetAutomovilByLegajoHandler(IAutomovilRepository repository) : IRequestQueryHandler<GetAutomovilByLegajoQuery, AutomovilDto>
    {
        private readonly IAutomovilRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<AutomovilDto> Handle(GetAutomovilByLegajoQuery query, CancellationToken cancellationToken)
        {
            var automovil = await _repository.GetByChasisAsync(query.NumeroChasis)?? throw new EntityDoesNotExistException();
            return automovil.To<AutomovilDto>();
        }
    }
}