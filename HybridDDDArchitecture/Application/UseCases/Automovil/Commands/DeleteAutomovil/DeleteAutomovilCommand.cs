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
    // IRequestCommand<bool> indica que el handler devolverá true si la eliminación fue exitosa.
    public class DeleteAutomovilCommand : IRequestCommand<bool>
    {
        public int AutomovilId { get; set; }
    }
}