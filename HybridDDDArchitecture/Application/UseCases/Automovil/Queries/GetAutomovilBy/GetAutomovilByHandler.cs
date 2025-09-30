using Application.DataTransferObjects;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;

namespace Application.UseCases.Automovil.Queries.GetAutomovilBy
{
    internal sealed class GetAutomovilByHandler(IAutomovilRepository repository) : IRequestQueryHandler<GetAutomovilByQuery, AutomovilDto>
    {
        private readonly IAutomovilRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<AutomovilDto> Handle(GetAutomovilByQuery query, CancellationToken cancellationToken)
        {
            var automovil = await _repository.GetByIdAsync(query.Id)?? throw new EntityDoesNotExistException();
            return automovil.To<AutomovilDto>();
        }
    }
}