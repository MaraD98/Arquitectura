using Core.Application;

namespace Application
{
    public class DummyEntityCreatedIntegrationEventHandlerSub : IIntegrationEventHandler<DummyEntityCreatedIntegrationEvent>
    {
        public Task Handle(DummyEntityCreatedIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
