using Core.Application;

namespace Application
{
    public class DummyEntityCreatedIntegrationEventHandlerPub : IIntegrationEventHandler<DummyEntityCreatedIntegrationEvent>
    {
        private readonly IEventBus _eventBus;

        public DummyEntityCreatedIntegrationEventHandlerPub(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public Task Handle(DummyEntityCreatedIntegrationEvent @event)
        {
            _eventBus.Publish(@event);

            return Task.CompletedTask;
        }
    }
}
