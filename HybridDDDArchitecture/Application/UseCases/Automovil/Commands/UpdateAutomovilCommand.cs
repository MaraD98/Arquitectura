using Core.Application;

namespace Application.UseCases.Automovil.Commands.UpdateAutomovil
{

    public class UpdateAutomovilCommand : IRequestCommand<bool>
    {
        public int Id { get; set; } // Identificador para saber qué actualizar
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public int Fabricacion { get; set; }
        public string NumeroMotor { get; set; }
        public string NumeroChasis { get; set; }
    }
}