using MediatR;
using System.Threading;
using System.Threading.Tasks;

// CORRECCIÓN IDE0130: Cambiamos el namespace para incluir la carpeta 'Buses'
namespace Core.Application.ComandQueryBus.Buses
{
    public interface ICommandQueryBus
    {
        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task Send(IRequest request, CancellationToken cancellationToken = default);
    }
}