using Application.Constants;
using Application.DomainEvents;
using Application.Exceptions;
using Application.Repositories;
using Application.UseCases.DummyEntity.Commands.DeleteDummyEntity;
using Core.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Commands.DeleteAutomovil
{
    internal sealed class DeleteAutomovilHandler(ICommandQueryBus domainBus, IAutomovilRepository automovilRepository)
         : IRequestCommandHandler<DeleteAutomovilCommand, Unit>
    {
        private readonly ICommandQueryBus _domainBus = domainBus ?? throw new ArgumentNullException(nameof(domainBus));
        private readonly IAutomovilRepository _context = automovilRepository ?? throw new ArgumentNullException(nameof(automovilRepository));

        public Task<Unit> Handle(DeleteAutomovilCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Remove(request.AutomovilId);

                _domainBus.Publish(new AutomovilEliminado(request.AutomovilId), cancellationToken);

                return Unit.Task;
            }
            catch (Exception ex)
            {
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex.InnerException);
            }
        }
    }
}
