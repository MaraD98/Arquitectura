using Application.Repositories;
using Core.Application;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Commands.CreateAutomovil
{
    // 🚨 CORRECCIÓN CS0311 / CS0118: El handler usa el tipo de retorno calificado Domain.Entities.Automovil
    internal class CreateAutomovilHandler(IAutomovilRepository automovilRepository)
        : IRequestCommandHandler<CreateAutomovilCommand, Domain.Entities.Automovil>
    {
        private readonly IAutomovilRepository _automovilRepository = automovilRepository;

        // El método Handle ahora también devuelve el tipo calificado
        public async Task<Domain.Entities.Automovil> Handle(CreateAutomovilCommand request, CancellationToken cancellationToken)
        {
            // Usamos el constructor corregido que solo toma 3 argumentos.
            var automovil = new Domain.Entities.Automovil(
                request.Marca,
                request.Modelo,
                request.Color
            );

            await _automovilRepository.AddAsync(automovil);

            return automovil;
        }
    }
}