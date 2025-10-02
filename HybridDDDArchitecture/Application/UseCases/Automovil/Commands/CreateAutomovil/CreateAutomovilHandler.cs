using Application.ApplicationServices;
using Application.Constants;
using Application.DomainEvents;
using Application.Exceptions;
using Application.Repositories;
using AutoMapper;
using Core.Application;
using Domain.Entities; // Necesitas importar Domain.Entities
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Commands.CreateAutomovil
{
    // Cambiamos el tipo de retorno de 'string' a 'AutomovilQueryResult'
    internal class CreateAutomovilHandler : IRequestCommandHandler<CreateAutomovilCommand, AutomovilQueryResult>
    {
        private readonly IAutomovilRepository _automovilRepository;
        private readonly IMapper _mapper; // Agregamos IMapper para mapear la entidad al DTO
        // NOTA: Se eliminaron _domainBus y _automovilApplicationService de aquí.
        // La lógica de negocio (reglas) se delega al dominio (entidad.IsValid) y la persistencia al repositorio.

        public CreateAutomovilHandler(
            IAutomovilRepository automovilRepository,
            IMapper mapper) // Inyectamos IMapper
        {
            _automovilRepository = automovilRepository ?? throw new ArgumentNullException(nameof(automovilRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AutomovilQueryResult> Handle(CreateAutomovilCommand request, CancellationToken cancellationToken)
        {
            // 1. Crear la entidad de dominio con todas las propiedades requeridas
            var entity = new Domain.Entities.Automovil(
                request.Marca,
                request.Modelo,
                request.Color,
                request.Fabricacion,
                request.NumeroMotor,
                request.NumeroChasis
            );

            // 2. Validación (si IsValid revisa todas las propiedades, incluyendo las que faltaban en tu DTO original)
            if (!entity.IsValid) throw new InvalidEntityDataException(entity.GetErrors());

            // 3. Persistir la entidad. AddAsync retorna el objeto persistido (con el ID) o su ID.
            await _automovilRepository.AddAsync(entity); // Guardar la entidad, que ahora debe tener su ID asignado

            // NOTA: Si necesitas la validación AutomovilExist, debería estar aquí o en un Pipeline Behaviour.
            // if (_automovilApplicationService.AutomovilExist(entity.NumeroChasis)) throw new EntityDoesExistException();

            try
            {
                // 4. Publicar el evento de dominio (si es necesario)
                // await _domainBus.Publish(entity.To<AutomovilCreado>(), cancellationToken);

                // 5. Mapear la entidad (que ya tiene el ID) al DTO de respuesta y retornarlo
                return _mapper.Map<AutomovilQueryResult>(entity);
            }
            catch (Exception ex)
            {
                // Reemplazado por una excepción más específica o propagación
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}