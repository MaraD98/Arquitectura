using Application.Repositories;
using Core.Application;
using Domain.Entities;
using Application.UseCases.Automovil.Commands.UpdateAutomovil;

namespace Application.UseCases.Automovil.Commands
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
            var existing = await _repository.FindOneAsync(request.Id);
            if (existing == null) return false;

            existing.Update(request.Marca, request.Modelo, request.Color, request.Fabricacion, request.NumeroMotor);

            if (!existing.IsValid) return false;

            await _repository.SaveAsync(existing);
            return true;
        }
    }
}