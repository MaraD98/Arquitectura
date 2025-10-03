using Application.DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

// Este namespace debe coincidir con el de la implementación (ApplicationServices)
namespace Application.ApplicationServices
{
    public interface IAutomovilApplicationService
    {
        Task<AutomovilDto> GetAutomovilByChasisAsync(string chasis);
        Task<IEnumerable<AutomovilDto>> GetAllAutomovilesAsync();
        Task<bool> AutomovilExist(string chasis);
    }
}