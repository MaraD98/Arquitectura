using Application.ApplicationServices;
using Application.Constants;
using Application.DomainEvents;
using Application.Exceptions;
using Application.Repositories;
using AutoMapper;
using Core.Application;
// Este using es correcto, ya que aquí está definido AutomovilDto.
using Application.DataTransferObjects;
using Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Commands.CreateAutomovil
{
    // CORRECCIÓN: Usamos el DTO correcto: AutomovilDto
    // Usamos el constructor primario para mayor limpieza.
    internal class CreateAutomovilHandler(IAutomovilRepository automovilRepository, IMapper mapper)
        : IRequestCommandHandler<CreateAutomovilCommand, AutomovilDto>
    {
        private readonly IAutomovilRepository _automovilRepository = automovilRepository;
        private readonly IMapper _mapper = mapper;

        // CORRECCIÓN: El método retorna Task<AutomovilDto>
        public async Task<AutomovilDto> Handle(CreateAutomovilCommand request, CancellationToken cancellationToken)
        {
            // 1. Crear la entidad de dominio
            var entity = new Domain.Entities.Automovil(
                request.Marca,
                request.Modelo,
                request.Color,
                request.Fabricacion,
                request.NumeroMotor,
                request.NumeroChasis
            );

            // 2. Validación
            if (!entity.IsValid) throw new InvalidEntityDataException(entity.GetErrors());

            // 3. Persistir
            await _automovilRepository.AddAsync(entity);

            try
            {
                // 4. Mapear y devolver el DTO
                // Mapeamos al DTO correcto: AutomovilDto
                return _mapper.Map<AutomovilDto>(entity);
            }
            catch (Exception ex)
            {
                // Propagamos la excepción
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex);
            }
        }
    }
}