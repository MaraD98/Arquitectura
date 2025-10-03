using Application.ApplicationServices;
using Application.Constants;
using Application.DomainEvents;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;

namespace Application.UseCases.Automovil.Commands.CreateAutomovil
{
    internal class CreateAutomovilHandler : IRequestCommandHandler<CreateAutomovilCommand, string>
    {
        private readonly ICommandQueryBus _domainBus;
        private readonly IAutomovilRepository _automovilRepository;
        private readonly IAutomovilApplicationService _automovilApplicationService;

        public CreateAutomovilHandler(
            ICommandQueryBus domainBus,
            IAutomovilRepository automovilRepository,
            IAutomovilApplicationService automovilApplicationService)
        {
            _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
            _automovilRepository = automovilRepository ?? throw new ArgumentNullException(nameof(automovilRepository));
            _automovilApplicationService = automovilApplicationService ?? throw new ArgumentNullException(nameof(automovilApplicationService));
        }

        public async Task<string> Handle(CreateAutomovilCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Automovil(request.Marca, request.Modelo, request.Color);

            if (!entity.IsValid) throw new InvalidEntityDataException(entity.GetErrors());

            if (_automovilApplicationService.AutomovilExist(entity.NumeroChasis)) throw new EntityDoesExistException();

            try
            {
                object createdId = await _automovilRepository.AddAsync(entity);

                await _domainBus.Publish(entity.To<AutomovilCreado>(), cancellationToken);

                return createdId.ToString();
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
