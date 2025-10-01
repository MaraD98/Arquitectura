using Core.Application;
using Application.DataTransferObjects;

namespace Application.UseCases.Automovil.Queries
{
    public class GetAutomovilByChasisQuery : IRequestQuery<AutomovilDto>
    {
        public string NumeroChasis { get; set; }
    }
}