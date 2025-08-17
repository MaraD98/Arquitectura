namespace Core.Application
{
    public interface IEventBusSubscriptionManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;
        void AddDynamicSubscription<THandler>(string eventName)
            where THandler : IDynamicIntegrationEventHandler;
        void AddSubscription<TEvent, THandler>()
          where TEvent : IntegrationEvent
          where THandler : IIntegrationEventHandler<TEvent>;
        void RemoveSubscription<TEvent, THandler>()
          where TEvent : IntegrationEvent
          where THandler : IIntegrationEventHandler<TEvent>;
        void RemoveDynamicSubscription<THandler>(string eventName)
            where THandler : IDynamicIntegrationEventHandler;
        bool HasSubscriptionsForEvent<TEvent>()
            where TEvent : IntegrationEvent;
        bool HasSubscriptionsForEvent(string eventName);
        Type GetEventTypeByName(string eventName);
        void Clear();
        IList<SubscriptionInfo> GetHandlersForEvent<TEvent>()
            where TEvent : IntegrationEvent;
        IList<SubscriptionInfo> GetHandlersForEvent(string eventName);
        string GetEventKey<T>();
    }
}
