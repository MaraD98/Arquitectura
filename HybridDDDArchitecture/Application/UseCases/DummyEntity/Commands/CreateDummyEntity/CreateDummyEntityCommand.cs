using Core.Application;
using System.ComponentModel.DataAnnotations;
using static Domain.Enums.Enums;

namespace Application.UseCases.DummyEntity.Commands.CreateDummyEntity
{
    /// <summary>
    /// Ejemplo de comando para crear una entidad de dominio Dummy
    /// Todo comando debe implementar la interfaz <see cref="IRequestCommand{TResponse}"/> 
    /// si espera una respuesta donde <c TResponse> puede ser cualquier tipo de dato, 
    /// o bien <see cref="IRequestCommand"/> si no espera un valor devuelto
    /// </summary>
    public class CreateDummyEntityCommand : IRequestCommand<string>
    {
        [Required]
        public string DummyPropertyTwo { get; set; }
        public DummyValues DummyPropertyThree { get; set; }

        public CreateDummyEntityCommand()
        {
        }
    }
}
