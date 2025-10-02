using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Commands.DeleteAutomovil
{
    internal class DeleteAutomovilHandler(IAutomovilRepository repository) : IRequestCommandHandler<DeleteAutomovilCommand, bool>
    {
        private readonly IAutomovilRepository _repository = repository;

        public async Task<bool> Handle(DeleteAutomovilCommand request, CancellationToken cancellationToken)
        {
            var automovil = await _repository.GetByIdAsync(request.AutomovilId);

            // 🚨 CORRECCIÓN IDE0270: Simplificación de la comprobación a 'is null'
            if (automovil is null)
            {
                throw new EntityDoesNotExistException($"Automóvil con ID {request.AutomovilId} no encontrado para eliminar.");
            }

            await _repository.DeleteAsync(automovil);

            return true;
        }
    }
}