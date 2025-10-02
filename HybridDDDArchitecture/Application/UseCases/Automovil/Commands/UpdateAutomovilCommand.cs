using Core.Application;

namespace Application.UseCases.Automovil.Commands.UpdateAutomovil
{
    // El IRequestCommand retornará un 'bool' para indicar éxito o fracaso, 
    // lo cual manejaremos con un 200 OK o un 400 Bad Request/404 Not Found en el controlador.
    public class UpdateAutomovilCommand : IRequestCommand<bool>
    {
        public int Id { get; set; }

        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public int Fabricacion { get; set; }
      
        public string NumeroMotor { get; set; }
        public string NumeroChasis { get; set; }
    }
}