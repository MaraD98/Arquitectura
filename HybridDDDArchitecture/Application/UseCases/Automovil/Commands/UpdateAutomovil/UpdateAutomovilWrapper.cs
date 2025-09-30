using Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Automovil.Commands.UpdateAutomovil
{
    public record UpdateAutomovilWrapper(int Id, UpdateAutomovilCommand Command) : IRequestCommand;
}
