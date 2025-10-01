using Application.DataTransferObjects;
using Core.Application;
using System.ComponentModel.DataAnnotations;


namespace Application.UseCases.Automovil.Queries.GetAutomovilByLegajo
{
    public class GetAutomovilByLegajoQuery : IRequestQuery<AutomovilDto>
    {
        [Required]
        public string NumeroChasis { get; set; }

        public GetAutomovilByLegajoQuery()
        {
        }
    }
}
