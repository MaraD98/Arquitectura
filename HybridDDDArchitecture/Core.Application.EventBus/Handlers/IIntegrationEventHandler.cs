namespace Core.Application
{
    public interface IIntegrationEventHandler
    {
    }

    public interface IIntegrationEventHandler<in TEvent> : IIntegrationEventHandler where TEvent : IntegrationEvent
    {
        Task Handle(TEvent @event);
    }

    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(object eventData);
    }
}
