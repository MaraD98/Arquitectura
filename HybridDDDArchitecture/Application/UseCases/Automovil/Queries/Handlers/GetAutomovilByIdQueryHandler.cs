// Ruta: C:\Users\cebre\OneDrive\Documentos\GitHub\Arquitectura\HybridDDDArchitecture\Application\UseCases\Automovil\Queries\Handlers\GetAutomovilByIdQueryHandler.cs

using Application.DataTransferObjects;
using Application.Repositories;
using Core.Application;
using AutoMapper;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Queries.Handlers
{
    public class GetAutomovilByIdQueryHandler(IAutomovilRepository automovilRepository, IMapper mapper) : IRequestQueryHandler<GetAutomovilByIdQuery, AutomovilDto>
    {
        private readonly IAutomovilRepository _automovilRepository = automovilRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<AutomovilDto> Handle(GetAutomovilByIdQuery request, CancellationToken cancellationToken)
        {
            // CORRECCIÓN CS1061: Cambiamos 'FindOneAsync' por 'GetByIdAsync' (o el nombre que realmente tiene en IAutomovilRepository)
            var automovil = await _automovilRepository.GetByIdAsync(request.Id);

            return _mapper.Map<AutomovilDto>(automovil);
        }
    }
}