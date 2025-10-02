using Application.DataTransferObjects;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Queries.Handlers
{
    public class GetAutomovilByIdQueryHandler(IAutomovilRepository automovilRepository, IMapper mapper) : IRequestQueryHandler<GetAutomovilByIdQuery, AutomovilDto>
    {
        private readonly IAutomovilRepository _automovilRepository = automovilRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<AutomovilDto> Handle(GetAutomovilByIdQuery request, CancellationToken cancellationToken)
        {
            var automovil = await _automovilRepository.GetByIdAsync(request.Id);

            if (automovil is null)
            {
                // Se lanza la excepción si no se encuentra (Requisito 4)
                throw new EntityDoesNotExistException($"Automóvil con ID {request.Id} no encontrado.");
            }

            return _mapper.Map<AutomovilDto>(automovil);
        }
    }
}