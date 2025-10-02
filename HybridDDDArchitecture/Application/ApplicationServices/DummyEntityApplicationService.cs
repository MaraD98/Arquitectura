using Application.DataTransferObjects;
using Application.Repositories;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ApplicationServices
{
    public class DummyEntityApplicationService(IDummyEntityRepository dummyEntityRepository, IMapper mapper) : IDummyEntityApplicationService
    {
        private readonly IDummyEntityRepository _dummyEntityRepository = dummyEntityRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<DummyEntityDto> GetDummyEntityByIdAsync(int id)
        {
            var dummyEntity = await _dummyEntityRepository.FindOneAsync(id);
            return _mapper.Map<DummyEntityDto>(dummyEntity);
        }

        public async Task<IEnumerable<DummyEntityDto>> GetAllDummyEntitiesAsync()
        {
            var dummyEntities = await _dummyEntityRepository.FindAllAsync();
            return _mapper.Map<IEnumerable<DummyEntityDto>>(dummyEntities);
        }

        // 🚨 Implementación requerida por la interfaz
        public async Task<bool> DummyEntityExistAsync(string id)
        {
            // Asumiendo que el ID de la entidad es un entero (int)
            if (int.TryParse(id, out int entityId))
            {
                var entity = await _dummyEntityRepository.FindOneAsync(entityId);
                return entity is not null;
            }
            return false;
        }
    }
}