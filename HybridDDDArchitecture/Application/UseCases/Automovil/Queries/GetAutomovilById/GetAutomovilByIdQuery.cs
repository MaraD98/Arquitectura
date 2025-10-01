using Application.DataTransferObjects;
using Core.Application;
using System.ComponentModel.DataAnnotations;


namespace Application.UseCases.Automovil.Queries.GetAutomovilBy
{
    public class GetAutomovilByIdQuery : IRequestQuery<AutomovilDto>
    {
        [Required]
        public int Id { get; set; }

        public GetAutomovilByIdQuery()
        {
        }
    }
}
