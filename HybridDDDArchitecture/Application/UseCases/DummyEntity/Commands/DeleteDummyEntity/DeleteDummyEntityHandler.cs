using Application.Constants;
using Application.DomainEvents;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using MediatR;

namespace Application.UseCases.DummyEntity.Commands.DeleteDummyEntity
{
    internal sealed class DeleteDummyEntityHandler(ICommandQueryBus domainBus, IDummyEntityRepository dummyEntityRepository)
        : IRequestCommandHandler<DeleteDummyEntityCommand, Unit>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IDummyEntityRepository _context = dummyEntityRepository ?? throw new ArgumentNullException(nameof(dummyEntityRepository));

        public Task<Unit> Handle(DeleteDummyEntityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Remove(request.DummyIdProperty);

                _domainBus.Publish(new DummyEntityDeleted(request.DummyIdProperty), cancellationToken);

                return Unit.Task;
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
