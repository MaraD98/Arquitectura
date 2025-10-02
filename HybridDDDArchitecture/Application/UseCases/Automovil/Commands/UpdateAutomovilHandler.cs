using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Commands.UpdateAutomovil
{
    internal class UpdateAutomovilHandler : IRequestCommandHandler<UpdateAutomovilCommand, bool>
    {
        private readonly IAutomovilRepository _repository;

        public UpdateAutomovilHandler(IAutomovilRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateAutomovilCommand request, CancellationToken cancellationToken)
        {
            // 1. Recuperar la entidad existente
            var automovil = await _repository.GetByIdAsync(request.Id);

            // Si no existe, lanzamos una excepción. El middleware la mapeará a 404 Not Found.
            if (automovil == null)
            {
                throw new EntityNotFoundException($"Automóvil con ID {request.Id} no encontrado para actualizar.");
            }

            // 2. Aplicar los cambios a través de un método de dominio
            automovil.UpdateProperties(
                request.Marca,
                request.Modelo,
                request.Color,
                request.Fabricacion,
                request.NumeroMotor,
                request.NumeroChasis
            );

            // 3. Revalidar la entidad después de la actualización (opcional pero recomendado)
            if (!automovil.IsValid)
            {
                throw new InvalidEntityDataException(automovil.GetErrors());
            }

            // 4. Persistir los cambios (UpdateAsync)
            await _repository.UpdateAsync(automovil);

            // Retornamos true para indicar éxito
            return true;
        }
    }
}