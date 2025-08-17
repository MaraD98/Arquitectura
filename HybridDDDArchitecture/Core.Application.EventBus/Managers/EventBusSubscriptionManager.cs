namespace Core.Application
{
    public class EventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _subscribedHandlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;
        public bool IsEmpty => _subscribedHandlers.Keys.Count == 0;

        public EventBusSubscriptionManager()
        {
            _subscribedHandlers = [];
            _eventTypes = [];
        }

        public void AddDynamicSubscription<THandler>(string eventName) where THandler : IDynamicIntegrationEventHandler
        {
            DoAddSubscription(typeof(THandler), eventName, isDynamic: true);
        }

        public void AddSubscription<TEvent, THandler>()
          where TEvent : IntegrationEvent
          where THandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();

            DoAddSubscription(typeof(THandler), eventName, isDynamic: false);

            if (_eventTypes.Contains(typeof(TEvent)))
                return;

            _eventTypes.Add(typeof(TEvent));
        }

        public void RemoveSubscription<TEvent, THandler>()
          where TEvent : IntegrationEvent
          where THandler : IIntegrationEventHandler<TEvent>
        {
            var handlerToRemove = FindSubscriptionToRemove<TEvent, THandler>();
            var eventName = GetEventKey<TEvent>();
            DoRemoveHandler(eventName, handlerToRemove);
        }

        public void RemoveDynamicSubscription<THandler>(string eventName) where THandler : IDynamicIntegrationEventHandler
        {
            SubscriptionInfo subscriptionToRemove = FindDynamicSubscriptionToRemove<THandler>(eventName);
            DoRemoveHandler(eventName, subscriptionToRemove);
        }

        public bool HasSubscriptionsForEvent<TEvent>() where TEvent : IntegrationEvent
        {
            var eventName = GetEventKey<TEvent>();
            return HasSubscriptionsForEvent(eventName);
        }

        public bool HasSubscriptionsForEvent(string eventName)
        {
            return _subscribedHandlers.ContainsKey(eventName);
        }

        public Type GetEventTypeByName(string eventName)
        {
            return _eventTypes.SingleOrDefault(eventType => eventType.Name == eventName)!;
        }

        public IList<SubscriptionInfo> GetHandlersForEvent<TEvent>() where TEvent : IntegrationEvent
        {
            var eventName = GetEventKey<TEvent>();
            return GetHandlersForEvent(eventName);
        }

        public IList<SubscriptionInfo> GetHandlersForEvent(string eventName)
        {
            return _subscribedHandlers[eventName];
        }

        public void Clear() => _subscribedHandlers.Clear();

        public string GetEventKey<TEvent>() => typeof(TEvent).Name;

        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _subscribedHandlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (_subscribedHandlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            if (isDynamic)
            {
                _subscribedHandlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
            }
            else
            {
                _subscribedHandlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
            }
        }

        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if(!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }
            
            return _subscribedHandlers[eventName].SingleOrDefault(eventType => eventType.HandlerType == handlerType);
        }

        private void DoRemoveHandler(string eventName, SubscriptionInfo subscriptionsToRemove)
        {
            if (subscriptionsToRemove != null)
            {
                _subscribedHandlers[eventName].Remove(subscriptionsToRemove);
                if(!_subscribedHandlers[eventName].Any())
                {
                    _subscribedHandlers.Remove(eventName);
                    var eventType = _eventTypes.SingleOrDefault(evType => evType.Name == eventName);
                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }

                    RaiseOnEventRemoved(eventName);
                }   
            }
        }

        private SubscriptionInfo FindSubscriptionToRemove<TEvent, THandler>()
             where TEvent : IntegrationEvent
             where THandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();
            return DoFindSubscriptionToRemove(eventName, typeof(THandler));
        }

        private SubscriptionInfo FindDynamicSubscriptionToRemove<THandler>(string eventName) 
            where THandler : IDynamicIntegrationEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(THandler));
        }

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }   
    }
}
