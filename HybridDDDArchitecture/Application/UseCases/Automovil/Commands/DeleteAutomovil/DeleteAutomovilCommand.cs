using Core.Application;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Automovil.Commands.DeleteAutomovil
{
    public class DeleteAutomovilCommand : IRequestCommand<Unit>
    {
        [Required]
        public int AutomovilId { get; set; }

        public DeleteAutomovilCommand()
        {
        }
    }
}