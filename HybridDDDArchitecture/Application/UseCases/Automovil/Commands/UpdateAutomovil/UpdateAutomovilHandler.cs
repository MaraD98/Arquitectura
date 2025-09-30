using Application.Constants;
using Application.DomainEvents;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;

namespace Application.UseCases.Automovil.Commands.UpdateAutomovil
{
    internal class UpdateAutomovilHandler(ICommandQueryBus domainBus, IAutomovilRepository automovilRepository) : IRequestCommandHandler<UpdateAutomovilWrapper>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IAutomovilRepository _context = automovilRepository ?? throw new ArgumentNullException(nameof(automovilRepository));

        public async Task Handle(UpdateAutomovilWrapper request, CancellationToken cancellationToken)
        {
            var entity = await _context.FindOneAsync(request.Id)?? throw new EntityDoesNotExistException();
            entity.Actualizar(
                request.Command.Marca,
                request.Command.Modelo,
                request.Command.Color,
                request.Command.Fabricacion,
                request.Command.NumeroMotor,
                request.Command.NumeroChasis
            );

            try
            {
                _context.Update(request.Id, entity);
                await _domainBus.Publish(entity.To<AutomovilActualizado>(), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
