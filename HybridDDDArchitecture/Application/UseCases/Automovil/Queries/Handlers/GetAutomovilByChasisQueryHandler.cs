// Ruta: C:\Users\cebre\OneDrive\Documentos\GitHub\Arquitectura\HybridDDDArchitecture\Application\UseCases\Automovil\Queries\Handlers\GetAutomovilByChasisQueryHandler.cs

using Application.DataTransferObjects;
using Application.Repositories;
using Core.Application;
using AutoMapper;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Queries.Handlers
{
    // Asumiendo que GetAutomovilByChasisQuery es la solicitud (IRequestQuery) y AutomovilDto la respuesta
    public class GetAutomovilByChasisQueryHandler(IAutomovilRepository automovilRepository, IMapper mapper) : IRequestQueryHandler<GetAutomovilByChasisQuery, AutomovilDto>
    {
        private readonly IAutomovilRepository _automovilRepository = automovilRepository;
        private readonly IMapper _mapper = mapper;

        // La clase GetAutomovilByChasisQuery debe estar definida en algún lugar
        public async Task<AutomovilDto> Handle(GetAutomovilByChasisQuery request, CancellationToken cancellationToken)
        {
            // CORRECCIÓN CS1998: Usamos 'await' para la llamada al repositorio, resolviendo el warning
            var automovil = await _automovilRepository.GetByChasisAsync(request.NumeroChasis);

            return _mapper.Map<AutomovilDto>(automovil);
        }
    }
}
















