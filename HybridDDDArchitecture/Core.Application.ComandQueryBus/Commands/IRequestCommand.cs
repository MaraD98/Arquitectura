using MediatR;

namespace Core.Application
{
    public interface IRequestCommand : IRequest
    {
    }

    public interface IRequestCommand<out TResponse> : IRequest<TResponse>
    {
    }
}
