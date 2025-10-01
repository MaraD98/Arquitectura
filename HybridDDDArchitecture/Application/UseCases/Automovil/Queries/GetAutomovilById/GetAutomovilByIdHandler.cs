using Application.DataTransferObjects;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;

namespace Application.UseCases.Automovil.Queries.GetAutomovilById
{
    internal sealed class GetAutomovilByIdHandler(IAutomovilRepository repository) : IRequestQueryHandler<GetAutomovilByIdQuery, AutomovilDto>
    {
        private readonly IAutomovilRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<AutomovilDto> Handle(GetAutomovilByIdQuery query, CancellationToken cancellationToken)
        {
            var automovil = await _repository.GetByIdAsync(query.Id)?? throw new EntityDoesNotExistException();
            return automovil.To<AutomovilDto>();
        }
    }
}