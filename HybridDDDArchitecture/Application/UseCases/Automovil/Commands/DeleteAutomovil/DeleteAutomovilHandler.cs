using Application.Exceptions; // <--- Este using es correcto para EntityDoesNotExistException
using Application.Repositories;
using Core.Application;
using System.Threading;
using System.Threading.Tasks;

// IMPORTANTE: Si las excepciones EntityNotFoundException, InvalidEntityDataException, etc.,
// no están en el namespace Application.Exceptions (que ya está referenciado),
// tienes que añadir el using donde realmente estén.

namespace Application.UseCases.Automovil.Commands.DeleteAutomovil
{
    // Usamos el constructor principal (IDE0290)
    internal class DeleteAutomovilHandler(IAutomovilRepository repository) : IRequestCommandHandler<DeleteAutomovilCommand, bool>
    {
        private readonly IAutomovilRepository _repository = repository;

        public async Task<bool> Handle(DeleteAutomovilCommand request, CancellationToken cancellationToken)
        {
            // 1. Recuperar la entidad por ID
            var automovil = await _repository.GetByIdAsync(request.AutomovilId);

            // 2. Verificar existencia (usando la sintaxis is null)
            if (automovil is null)
            {
                // **CORRECCIÓN:** El nombre de la clase es EntityDoesNotExistException, no EntityNotFoundException.
                throw new EntityDoesNotExistException($"Automóvil con ID {request.AutomovilId} no encontrado para eliminar.");
            }

            // 3. Eliminar la entidad
            await _repository.DeleteAsync(automovil);

            return true;
        }
    }
}