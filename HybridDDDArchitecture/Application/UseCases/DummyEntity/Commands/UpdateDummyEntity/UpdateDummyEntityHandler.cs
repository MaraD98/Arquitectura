using Application.Constants;
using Application.DomainEvents;
using Application.Exceptions;
using Application.Repositories;
using Application.UseCases.DummyEntity.Commands.UpdateDummyEntity;
using Core.Application;

namespace ESCMB.Application.UseCases.DummyEntity.Commands.UpdateDummyEntity
{
    internal sealed class UpdateDummyEntityHandler(ICommandQueryBus domainBus, IDummyEntityRepository dummyEntityRepository) : IRequestCommandHandler<UpdateDummyEntityCommand>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IDummyEntityRepository _context = dummyEntityRepository ?? throw new ArgumentNullException(nameof(dummyEntityRepository));

        public async Task Handle(UpdateDummyEntityCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.DummyEntity entity = await _context.FindOneAsync(request.DummyIdProperty) ?? throw new EntityDoesNotExistException();
            entity.SetdummyPropertyOne(request.dummyPropertyOne);
            entity.SetdummyPropertyTwo(request.dummyPropertyTwo);

            try
            {
                _context.Update(request.DummyIdProperty, entity);

                await _domainBus.Publish(entity.To<DummyEntityUpdated>(), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
