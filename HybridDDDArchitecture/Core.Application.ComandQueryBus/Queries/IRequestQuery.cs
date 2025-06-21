using MediatR;

namespace Core.Application
{
    public interface IRequestQuery : IRequest
    {
    }

    public interface IRequestQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
