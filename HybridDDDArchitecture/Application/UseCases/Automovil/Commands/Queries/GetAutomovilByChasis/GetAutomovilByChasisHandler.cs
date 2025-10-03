using Application.DataTransferObjects;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;


namespace Application.UseCases.Automovil.Commands.Queries.GetAutomovilByChasis
{
    internal class GetAutomovilByChasisHandler(IAutomovilRepository repository) : IRequestQueryHandler<GetAutomovilByChasisQuery, AutomovilDto>
    {
        private readonly IAutomovilRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<AutomovilDto> Handle(GetAutomovilByChasisQuery query, CancellationToken cancellationToken)
        {
            var automovil = await _repository.GetByChasisAsync(query.NumeroChasis) ?? throw new EntityDoesNotExistException();
            return automovil.To<AutomovilDto>();
        }
    }
}
