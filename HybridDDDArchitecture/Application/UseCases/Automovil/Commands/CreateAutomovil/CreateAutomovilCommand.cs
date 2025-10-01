using Core.Application;

namespace Application.UseCases.Automovil.Commands.CreateAutomovil
{
    public class CreateAutomovilCommand : IRequestCommand<string>
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
    }

}
