using Application.DataTransferObjects;
using Application.Repositories;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

// Usar el namespace del mismo ApplicationService para que encuentre su interfaz
using Application.ApplicationServices;

namespace Application.ApplicationServices
{
    // CORRECCIÓN CS0246
    // Asumimos que los métodos GetByChasisAsync y FindAllAsync ahora son únicos.
    public class AutomovilApplicationService(IAutomovilRepository automovilRepository, IMapper mapper) : IAutomovilApplicationService
    {
        private readonly IAutomovilRepository _automovilRepository = automovilRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<AutomovilDto> GetAutomovilByChasisAsync(string chasis)
        {
            var automovil = await _automovilRepository.GetByChasisAsync(chasis);
            return _mapper.Map<AutomovilDto>(automovil);
        }

        public async Task<IEnumerable<AutomovilDto>> GetAllAutomovilesAsync()
        {
            // Nota: Si 'FindAllAsync' estaba en IRepository, también debe estar implementado
            // en AutomovilRepository, pero aquí lo llamamos directamente.
            var automoviles = await _automovilRepository.FindAllAsync();
            return _mapper.Map<IEnumerable<AutomovilDto>>(automoviles);
        }

        public async Task<bool> AutomovilExist(string chasis)
        {
            return await _automovilRepository.AutomovilExistsAsync(chasis);
        }
    }
}