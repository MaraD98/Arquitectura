using Application.DataTransferObjects;
using Application.Repositories;
using Core.Application;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Application.UseCases.Automovil.Queries.Handlers
{
    internal class GetAllAutomovilesQueryHandler(IAutomovilRepository repository, IMapper mapper) : IRequestQueryHandler<GetAllAutomovilesQuery, IEnumerable<AutomovilDto>>
    {
        private readonly IAutomovilRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<AutomovilDto>> Handle(GetAllAutomovilesQuery request, CancellationToken cancellationToken)
        {
            var automoviles = await _repository.FindAllAsync();

            // 🚨 CORRECCIÓN IDE0305: Simplificación de la inicialización
            return automoviles.Select(_mapper.Map<AutomovilDto>).ToList();
        }
    }
}