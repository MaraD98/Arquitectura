using Application.DataTransferObjects;
using Application.Repositories;
using Core.Application;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Queries.Handlers
{
    internal class GetAllAutomovilesQueryHandler : IRequestQueryHandler<GetAllAutomovilesQuery, IEnumerable<AutomovilDto>>
    {
        private readonly IAutomovilRepository _repository;

        public GetAllAutomovilesQueryHandler(IAutomovilRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AutomovilDto>> Handle(GetAllAutomovilesQuery request, CancellationToken cancellationToken)
        {
            // Usar FindAllAsync que sí existe en IRepository
            var automoviles = await _repository.FindAllAsync();
            
            return automoviles.Select(a => new AutomovilDto
            {
 














               Id = a.Id,
                Marca = a.Marca,
                Modelo = a.Modelo,
                Color = a.Color,
                Fabricacion = a.Fabricacion,
                NumeroMotor = a.NumeroMotor,
                NumeroChasis = a.NumeroChasis
            });
        }
    }
}
