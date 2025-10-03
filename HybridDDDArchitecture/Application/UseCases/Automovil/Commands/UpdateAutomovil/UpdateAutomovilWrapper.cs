using Core.Application;

namespace Application.UseCases.Automovil.Commands.UpdateAutomovil
{
    public record UpdateAutomovilWrapper(int Id, UpdateAutomovilCommand Command) : IRequestCommand;
}
