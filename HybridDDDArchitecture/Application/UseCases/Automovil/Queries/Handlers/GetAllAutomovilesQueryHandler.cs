using Application.DataTransferObjects;
using Application.Repositories;
using Core.Application;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper; // Asumo que también usas Mapper aquí

namespace Application.UseCases.Automovil.Queries.Handlers
{
    internal class GetAllAutomovilesQueryHandler(IAutomovilRepository repository, IMapper mapper) : IRequestQueryHandler<GetAllAutomovilesQuery, IEnumerable<AutomovilDto>>
    {
        private readonly IAutomovilRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<AutomovilDto>> Handle(GetAllAutomovilesQuery request, CancellationToken cancellationToken)
        {
            // Usa FindAllAsync que devuelve IEnumerable<Automovil> (Requisito 6)
            var automoviles = await _repository.FindAllAsync();

            // Mapea la colección completa a DTOs
            return automoviles.Select(a => _mapper.Map<AutomovilDto>(a)).ToList();
        }
    }
}