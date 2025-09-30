using Application.Constants;
using Application.DomainEvents;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Automovil.Commands.UpdateAutomovil
{
    internal class UpdateAutomovilHandler(ICommandQueryBus domainBus, IAutomovilRepository automovilRepository, ILogger<UpdateAutomovilHandler> logger) : IRequestCommandHandler<UpdateAutomovilWrapper>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IAutomovilRepository _context = automovilRepository ?? throw new ArgumentNullException(nameof(automovilRepository));
        private readonly ILogger<UpdateAutomovilHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


        public async Task Handle(UpdateAutomovilWrapper request, CancellationToken cancellationToken)
        {
            var entity = await _context.FindOneAsync(request.Id)?? throw new EntityDoesNotExistException();


            if (!string.IsNullOrWhiteSpace(request.Command.NumeroMotor))
            {
                if (!entity.EsNumeroMotorValido(request.Command.NumeroMotor))
                    throw new BussinessException("El número de motor no cumple con el formato requerido. Debe contener al menos 8 caracteres y empezar con MTR-");

                bool existe = await _context.ExisteNumeroMotorAsync(request.Command.NumeroMotor, request.Id);

                if (existe)
                    throw new BussinessException("El número de motor ya está registrado en otro vehículo.");
            }
            var cambios = entity.Actualizar(
            request.Command.Marca,
            request.Command.Modelo,
            request.Command.Color,
            request.Command.Fabricacion,
            request.Command.NumeroMotor
            );
            try
            {
                if (cambios.Count == 0)
                {
                    _logger.LogInformation($"Automóvil {request.Id} recibió una solicitud de actualización, pero no se modificó ningún campo.");
                    throw new BussinessException("No se modificó ningún campo del automóvil.");
                }
            
                _context.Update(request.Id, entity);
                await _domainBus.Publish(new AutomovilActualizado
                {
                    AutomovilId = request.Id,
                    CamposModificados = cambios,
                    FechaActualizacion = DateTime.UtcNow
                }, cancellationToken);

                _logger.LogInformation($"Automóvil {request.Id} actualizado correctamente. Campos modificados: {string.Join(", ", cambios)}");

            }
            catch (BussinessException)
            {
                // Re-lanzar directamente sin envolver
                throw;
            }
            catch (Exception ex)
            {
                // Solo envolver errores técnicos
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex);
            }
        }
    }
}
