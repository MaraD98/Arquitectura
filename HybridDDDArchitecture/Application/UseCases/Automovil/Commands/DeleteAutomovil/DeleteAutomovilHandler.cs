using Application.Constants;
using Application.DomainEvents;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using MediatR;

namespace Application.UseCases.Automovil.Commands.DeleteAutomovil
{
    internal class DeleteAutomovilHandler : IRequestCommandHandler<DeleteAutomovilCommand, Unit>
    {
        private readonly ICommandQueryBus _domainBus;
        private readonly IAutomovilRepository _automovilRepository;

        public DeleteAutomovilHandler(ICommandQueryBus domainBus, IAutomovilRepository automovilRepository)
        {
            _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
            _automovilRepository = automovilRepository ?? throw new ArgumentNullException(nameof(automovilRepository));
        }

        public async Task<Unit> Handle(DeleteAutomovilCommand request, CancellationToken cancellationToken)
        {
            var automovil = await _automovilRepository.GetByIdAsync(request.AutomovilId);

            if (automovil is null)
                throw new EntityDoesNotExistException();

            try
            {
                await _automovilRepository.DeleteAsync(automovil);

                await _domainBus.Publish(automovil.To<AutomovilEliminado>(), cancellationToken);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
