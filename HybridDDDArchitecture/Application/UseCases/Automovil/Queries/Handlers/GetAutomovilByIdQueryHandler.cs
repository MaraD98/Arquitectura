using Application.DataTransferObjects;
using Application.Repositories;
using Core.Application;
using Domain.Entities;

namespace Application.UseCases.Automovil.Queries.Handlers
{
    internal class GetAutomovilByIdQueryHandler : IRequestQueryHandler<GetAutomovilByIdQuery, AutomovilDto>
    {
        private readonly IAutomovilRepository _repository;

        public GetAutomovilByIdQueryHandler(IAutomovilRepository repository)
        {
            _repository = repository;
        }

        public async Task<AutomovilDto> Handle(GetAutomovilByIdQuery request, CancellationToken cancellationToken)
        {
            // Usar FindOneAsync que sí existe en IRepository
            var automovil = await _repository.FindOneAsync(request.Id);
            if (automovil == null) return null;

 











           return new AutomovilDto
            {
                Id = automovil.Id,
                Marca = automovil.Marca,
                Modelo = automovil.Modelo,
                Color = automovil.Color,
                Fabricacion = automovil.Fabricacion,
                NumeroMotor = automovil.NumeroMotor,
                NumeroChasis = automovil.NumeroChasis
            };
        }
    }
}
