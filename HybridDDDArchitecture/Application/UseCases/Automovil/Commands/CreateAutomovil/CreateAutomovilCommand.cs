using Core.Application;
using Application.DataTransferObjects;

namespace Application.UseCases.Automovil.Commands.CreateAutomovil
{

    public class CreateAutomovilCommand : IRequestCommand<Domain.Entities.Automovil>
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public int Fabricacion { get; set; }
        public string NumeroMotor { get; set; }
        public string NumeroChasis { get; set; }
    }
}

