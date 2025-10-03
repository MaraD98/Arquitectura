using Core.Application;
using Application.DataTransferObjects;

namespace Application.UseCases.Automovil.Queries
{
    public class GetAutomovilByIdQuery : IRequestQuery<AutomovilDto>
    {
        public int Id { get; set; }
    }
}