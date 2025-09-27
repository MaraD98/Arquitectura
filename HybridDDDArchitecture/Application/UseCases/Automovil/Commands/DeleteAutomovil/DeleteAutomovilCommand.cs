using Core.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Commands.DeleteAutomovil
{
    public class DeleteAutomovilCommand : IRequestCommand<Unit>
    {
        [Required]
        public int AutomovilId { get; set; }

        public DeleteAutomovilCommand()
        {
        }
        //public DeleteAutomovilCommand(int automovilId) podria agregar
        //{
        //    AutomovilId = automovilId;
        //}
    }
}