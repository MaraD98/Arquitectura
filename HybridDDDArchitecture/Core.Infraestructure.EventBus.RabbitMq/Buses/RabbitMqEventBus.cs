using Core.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Core.Infraestructure
{
    public class RabbitMqEventBus : IEventBus, IDisposable
    {
        private bool _disposedValue;
        string _queueName;
        private IModel _consumersChannel;

        private readonly int _retryCount;
        private readonly string _exchange;
        private readonly IConnectionManager _connectionManager;
        private readonly ILogger<RabbitMqEventBus> _logger;
        private readonly IEventBusSubscriptionManager _eventBusSubscriptionManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMqEventBus(
          IConnectionManager connectionManager,
          ILogger<RabbitMqEventBus> logger,
          IEventBusSubscriptionManager eventBusSubscriptionManager,
          IServiceScopeFactory serviceScopeFactory,
          string queueName = "CommonEventBus",
          int retryCont = 3)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBusSubscriptionManager = eventBusSubscriptionManager ?? throw new ArgumentNullException(nameof(eventBusSubscriptionManager));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _retryCount = retryCont;
            _queueName = queueName;
            _exchange = "CommonEventExchange";
            _consumersChannel = CreateConsumersChannel();
            _eventBusSubscriptionManager.OnEventRemoved += OnEventRemoved;
        }

        ~RabbitMqEventBus() => this.Dispose(false);

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        public void OnEventRemoved(object sender, string eventName)
        {
            this.SetConnection();
            using (IModel model = _connectionManager.CreateModel())
            {
                model.QueueUnbind(_queueName, _exchange, eventName);

                if (_eventBusSubscriptionManager.IsEmpty)
                {
                    _queueName = string.Empty;
                    _consumersChannel.Close();
                }
            }
        }

        public void Publish(IntegrationEvent @event)
        {
            SetConnection();

            RetryPolicy retryPolicy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });
            string eventName = @event.GetType().Name;

            _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using (IModel channel = _connectionManager.CreateModel())
            {
                channel.ExchangeDeclare(_exchange, "direct", durable:true, autoDelete: false);

                var message = JsonConvert.SerializeObject(@event);
                byte[] body = Encoding.UTF8.GetBytes(message);

                retryPolicy.Execute(() =>
                {
                    IBasicProperties basicProperties = channel.CreateBasicProperties();
                    basicProperties.DeliveryMode = 2;

                    _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);

                    channel.BasicPublish(exchange: _exchange, routingKey: eventName, mandatory: true, basicProperties, body);
                });
            }
        }

        public void SubscribeDynamic<THandler>(string eventName) where THandler : IDynamicIntegrationEventHandler
        {
            DoInternalSubscription(eventName);
            _eventBusSubscriptionManager.AddDynamicSubscription<THandler>(eventName);
            StartBasicConsume();
        }

        public void Subscribe<TEvent, THandler>()
          where TEvent : IntegrationEvent
          where THandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = _eventBusSubscriptionManager.GetEventKey<TEvent>();
            DoInternalSubscription(eventName);

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(THandler).GetGenericTypeName());

            _eventBusSubscriptionManager.AddSubscription<TEvent, THandler>();
            StartBasicConsume();
        }

        public void SetConnection()
        {
            if (_connectionManager.IsConnected)
                return;
            _connectionManager.TryConnect();
        }

        public void Unsubscribe<TEvent, THandler>()
          where TEvent : IntegrationEvent
          where THandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = _eventBusSubscriptionManager.GetEventKey<TEvent>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _eventBusSubscriptionManager.RemoveSubscription<TEvent, THandler>();
        }

        public void UnsubscribeDynamic<THandler>(string eventName) where THandler : IDynamicIntegrationEventHandler
        {
            this._eventBusSubscriptionManager.RemoveDynamicSubscription<THandler>(eventName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }
                
            if (disposing)
            {
                if (this._consumersChannel != null)
                {
                    _consumersChannel.Dispose();
                }
                    
                _eventBusSubscriptionManager.Clear();
            }
            _disposedValue = true;
        }

        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs args)
        {
            string eventName = args.RoutingKey;
            string message = Encoding.UTF8.GetString(args.Body.ToArray());
            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }
           
            _consumersChannel.BasicAck(args.DeliveryTag, false);
        }

        private IModel CreateConsumersChannel()
        {
            SetConnection();

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            IModel channel = _connectionManager.CreateModel();
            channel.ExchangeDeclare(_exchange, "direct", true, false);

            channel.QueueDeclare(_queueName, true, false, false, null);

            channel.CallbackException += (sender, args) =>
            {
                _logger.LogWarning(args.Exception, "Recreating RabbitMQ consumer channel");

                _consumersChannel.Dispose();
                _consumersChannel = CreateConsumersChannel();
                StartBasicConsume();
            };
            
            return channel;
        }

        private void DoInternalSubscription(string eventName)
        {
            var containerKey = _eventBusSubscriptionManager.HasSubscriptionsForEvent(eventName);
            if (!containerKey)
            {
                SetConnection();

                using(IModel channel = _connectionManager.CreateModel())
                {
                    channel.QueueBind(_queueName, _exchange, eventName);
                }
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_eventBusSubscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    IList<SubscriptionInfo> subscribedHandlers = _eventBusSubscriptionManager.GetHandlersForEvent(eventName);
                    foreach (var subscribedHandler in subscribedHandlers)
                    {
                        if (subscribedHandler.IsDynamic)
                        {
                            IDynamicIntegrationEventHandler handler = scope.ServiceProvider.GetService(subscribedHandler.HandlerType) as IDynamicIntegrationEventHandler;
                            if (handler == null)
                            {
                                continue;
                            }

                            dynamic eventData = JObject.Parse(message);

                            await Task.Yield();
                            await handler.Handle(eventData).ConfigureAwait(false);
                        }
                        else
                        {
                            var handler = scope.ServiceProvider.GetService(subscribedHandler.HandlerType);
                            if (handler is null)
                            {
                                continue;
                            }
                            var eventType = _eventBusSubscriptionManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                            await Task.Yield();
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });

                        }
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }

        private void StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumersChannel != null)
            {
                AsyncEventingBasicConsumer eventingBasicConsumer = new AsyncEventingBasicConsumer(_consumersChannel);
                eventingBasicConsumer.Received += ConsumerReceived;

                _consumersChannel.BasicConsume(_queueName, false, eventingBasicConsumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }
    }
}
