using Application.DataTransferObjects;
using Core.Application;
using System.ComponentModel.DataAnnotations;


namespace Application.UseCases.Automovil.Commands.Queries.GetAutomovilByChasis
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
