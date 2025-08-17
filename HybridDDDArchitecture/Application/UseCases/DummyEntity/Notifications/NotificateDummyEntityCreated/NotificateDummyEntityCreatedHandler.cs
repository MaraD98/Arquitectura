using Application.Constants;
using Application.DomainEvents;
using Core.Application;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.DummyEntity.Notifications.NotificateDummyEntityCreated
{
    internal class NotificateDummyEntityCreatedHandler : INotificationHandler<DummyEntityCreated>
    {
        private readonly ILogger<NotificateDummyEntityCreatedHandler> _logger;
        private readonly IIntegrationEventHandler<DummyEntityCreatedIntegrationEvent> _integrationEventHandler;
        public NotificateDummyEntityCreatedHandler(ILogger<NotificateDummyEntityCreatedHandler> logger, IIntegrationEventHandler<DummyEntityCreatedIntegrationEvent> integrationEventHandler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventHandler = integrationEventHandler ?? throw new ArgumentNullException(nameof(integrationEventHandler));
        }

        public async Task Handle(DummyEntityCreated notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new DummyEntityCreatedIntegrationEvent(
                nameof(DummyEntityCreated),
                ApplicationConstants.SERVICE_NAME,
                notification);

            await _integrationEventHandler.Handle(integrationEvent).ConfigureAwait(false);
        }
    }
}
