// CORRECCIÓN CS0246: Agregamos el using correcto para la excepción
using Application.Exceptions;
using Application.Repositories;
using Application.UseCases.Automovil.Commands.UpdateAutomovil;
using Core.Application;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

// El namespace original era Application.UseCases.Automovil.Commands.UpdateAutomovil,
// lo simplificamos para que contenga todas las clases de Commands.
namespace Application.UseCases.Automovil.Commands
{
    internal class UpdateAutomovilHandler(IAutomovilRepository repository) : IRequestCommandHandler<UpdateAutomovilCommand, bool>
    {
        private readonly IAutomovilRepository _repository = repository;

        public async Task<bool> Handle(UpdateAutomovilCommand request, CancellationToken cancellationToken)
        {
            // 1. Recuperar la entidad existente
            var automovil = await _repository.GetByIdAsync(request.Id);

            // 2. Verificar existencia
            // CORRECCIÓN CS0246: Usamos el nombre de la excepción definida en Exceptions.cs
            if (automovil == null)
            {
                throw new EntityDoesNotExistException($"Automóvil con ID {request.Id} no encontrado para actualizar.");
            }

            // 3. Actualizar propiedades del dominio
            automovil.UpdateProperties(
                request.Marca,
                request.Modelo,
                request.Color,
                request.Fabricacion,
                request.NumeroMotor,
                request.NumeroChasis
            );

            // Si tienes validaciones de dominio:
            // if (!automovil.IsValid)
            // {
            //     throw new InvalidEntityDataException(automovil.GetErrors()); 
            // }

            // 4. Persistir la actualización
            await _repository.UpdateAsync(automovil);

            return true;
        }
    }
}