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
            // 1. Recuperar la entidad por ID
            var automovil = await _repository.GetByIdAsync(request.AutomovilId);

            if (automovil is null)
            {
                // Lanza excepción si no existe
                throw new EntityDoesNotExistException($"Automóvil con ID {request.AutomovilId} no encontrado para eliminar.");
            }

            // 2. Eliminar la entidad
            await _repository.DeleteAsync(automovil);

            return true; // Éxito en la eliminación
        }
    }
}