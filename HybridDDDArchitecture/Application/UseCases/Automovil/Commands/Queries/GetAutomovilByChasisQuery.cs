
using Application.DataTransferObjects;
using Core.Application;
using System.ComponentModel.DataAnnotations;


namespace Application.UseCases.Automovil.Queries.GetAutomovilByChasis
{
    public class GetAutomovilByChasisQuery : IRequestQuery<AutomovilDto>
    {
        [Required]
        public string NumeroChasis { get; set; }

        public GetAutomovilByChasisQuery()
        {
        }
    }
}
