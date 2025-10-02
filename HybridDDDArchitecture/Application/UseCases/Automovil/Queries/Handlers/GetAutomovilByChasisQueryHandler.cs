using Application.DataTransferObjects;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Queries.Handlers
{
    public class GetAutomovilByChasisQueryHandler(IAutomovilRepository automovilRepository, IMapper mapper) : IRequestQueryHandler<GetAutomovilByChasisQuery, AutomovilDto>
    {
        private readonly IAutomovilRepository _automovilRepository = automovilRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<AutomovilDto> Handle(GetAutomovilByChasisQuery request, CancellationToken cancellationToken)
        {
            var automovil = await _automovilRepository.GetByChasisAsync(request.NumeroChasis);

            if (automovil is null)
            {
                // Se lanza la excepción si no se encuentra (Requisito 5)
                throw new EntityDoesNotExistException($"Automóvil con chasis {request.NumeroChasis} no encontrado.");
            }

            return _mapper.Map<AutomovilDto>(automovil);
        }
    }
}















