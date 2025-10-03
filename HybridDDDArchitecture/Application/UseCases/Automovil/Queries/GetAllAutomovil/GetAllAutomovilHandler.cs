using Application.DataTransferObjects;
using Application.Repositories;
using Core.Application;
namespace Application.UseCases.Automovil.Queries.GetAllAutomovil
{
    internal sealed class GetAllAutomovilHandler(IAutomovilRepository repository)
    : IRequestQueryHandler<GetAllAutomovilQuery, QueryResult<AutomovilDto>>
    {
        private readonly IAutomovilRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        public async Task<QueryResult<AutomovilDto>> Handle(GetAllAutomovilQuery query, CancellationToken cancellationToken)
        {
            IList<Domain.Entities.Automovil> automoviles = await _repository.FindAllAsync();

            return new QueryResult<AutomovilDto>(automoviles.To<AutomovilDto>(), automoviles.Count, query.PageSize, query.PageIndex);
        }
    }
}
