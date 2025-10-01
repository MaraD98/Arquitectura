using Application.DataTransferObjects;
using Application.Repositories;
using Core.Application;
using System.Linq;

namespace Application.UseCases.Automovil.Queries.Handlers
{
    internal class GetAutomovilByChasisQueryHandler : IRequestQueryHandler<GetAutomovilByChasisQuery, AutomovilDto>
    {
        private readonly IAutomovilRepository _repository;

        public GetAutomovilByChasisQueryHandler(IAutomovilRepository repository)
        {
            _repository = repository;
        }

        public async Task<AutomovilDto> Handle(GetAutomovilByChasisQuery request, CancellationToken cancellationToken)
        {
            var automovil = _repository.Query()
 

               .FirstOrDefault(a => a.NumeroChasis == request.NumeroChasis);
                
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
















