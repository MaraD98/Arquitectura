using RabbitMQ.Client;

namespace Core.Infraestructure
{
    public interface IConnectionManager : IDisposable
    {
        bool IsConnected { get; }

        IModel CreateModel();

        bool TryConnect();
    }
}
