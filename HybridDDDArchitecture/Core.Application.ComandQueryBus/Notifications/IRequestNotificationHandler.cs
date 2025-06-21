using MediatR;

namespace Core.Application
{
    public interface IRequestNotificationHandler<in TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
    }
}
