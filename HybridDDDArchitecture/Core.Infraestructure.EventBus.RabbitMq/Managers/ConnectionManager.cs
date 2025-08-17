using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace Core.Infraestructure
{
    public class ConnectionManager : IConnectionManager, IDisposable
    {
        public readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<ConnectionManager> _logger;
        private IConnection _connection;
        private readonly int _retryCount;
        private readonly object sync_root = new();
        private bool disposedValue;

        public bool IsConnected
        {
            get
            {
                DisposeCheck();
                return _connection != null && _connection.IsOpen && !disposedValue;
            }
        }

        public ConnectionManager(IConnectionFactory connectionFactory, ILogger<ConnectionManager> logger, int retryCount = 3)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;
        }

        ~ConnectionManager() => Dispose(false);

        public IModel CreateModel()
        {
            try
            {
                DisposeCheck();

                if (!IsConnected)
                {
                    if (!TryConnect())
                    {
                        throw new InvalidOperationException("No se pudo establecer la conexión con RabbitMQ");
                    }
                }

                var model = _connection.CreateModel();
                if (model == null)
                {
                    throw new InvalidOperationException("No se pudo crear el modelo de RabbitMQ");
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al crear el modelo de RabbitMQ", ex);
            }
        }

        public void Dispose()
        {
            if (disposedValue) return;

            disposedValue = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

        public void DisposeCheck()
        {
            ObjectDisposedException.ThrowIf(disposedValue, nameof(ConnectionManager));
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            lock (sync_root)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                    });

                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutDown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);

                    return true;
                }
                else
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

                    return false;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
                return;
            if (disposing)
            {
                try
                {
                    ((IDisposable)_connection).Dispose();
                }
                catch (IOException ex)
                {
                    throw new IOException(ex.Message);
                }
            }
            disposedValue = true;
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs args)
        {
            if (disposedValue)
                return;
            TryConnect();
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs args)
        {
            if (disposedValue)
                return;
            TryConnect();
        }

        private void OnConnectionShutDown(object sender, ShutdownEventArgs args)
        {
            if (disposedValue)
                return;
            TryConnect();
        }
    }
}
