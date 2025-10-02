using Application.DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ApplicationServices
{
    public interface IDummyEntityApplicationService
    {
        Task<DummyEntityDto> GetDummyEntityByIdAsync(int id);
        Task<IEnumerable<DummyEntityDto>> GetAllDummyEntitiesAsync();

        // 🚨 CORRECCIÓN CS1061: Se agrega el método asíncrono que faltaba.
        Task<bool> DummyEntityExistAsync(string id);
    }
}