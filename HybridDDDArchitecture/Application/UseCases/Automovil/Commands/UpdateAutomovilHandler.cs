using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Commands.UpdateAutomovil
{
    internal class UpdateAutomovilHandler(IAutomovilRepository repository)
        : IRequestCommandHandler<UpdateAutomovilCommand, bool>
    {
        private readonly IAutomovilRepository _repository = repository;

        public async Task<bool> Handle(UpdateAutomovilCommand request, CancellationToken cancellationToken)
        {
            var automovil = await _repository.GetByIdAsync(request.Id);

            // 🚨 CORRECCIÓN IDE0270: Simplificación de la comprobación a 'is null'
            if (automovil is null)
            {
                throw new EntityDoesNotExistException($"Automóvil con ID {request.Id} no encontrado para actualizar.");
            }

            // 🚨 CORRECCIÓN CRÍTICA CS1503: Se llama al método solo con el argumento 'Color'
            automovil.UpdateProperties(request.Color);

            await _repository.UpdateAsync(automovil);

            return true;
        }
    }
}