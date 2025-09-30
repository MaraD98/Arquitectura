using Application.UseCases.Automovil.Commands.CreateAutomovil;
using Application.UseCases.Automovil.Commands.DeleteAutomovil;
using Application.UseCases.Automovil.Commands.UpdateAutomovil;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    public class AutomovilController : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus;

        public AutomovilController(ICommandQueryBus commandQueryBus)
        {
            _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));
        }

        [HttpPost("api/v1/[controller]")]
        public async Task<IActionResult> Create(CreateAutomovilCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/v1/[controller]/{id}", new
            {
                Id = id,
                mensaje = "Automovil creado correctamente"
            });
        }

        [HttpDelete("api/v1/[controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("ID inválido.");

            await _commandQueryBus.Send(new DeleteAutomovilCommand { AutomovilId = id });

            return Ok(new { mensaje = "Automovil eliminado correctamente" });
        }

        [HttpPut("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAutomovilCommand command)
        {
            await _commandQueryBus.Send(new UpdateAutomovilWrapper(id, command));

            return Ok(new { mensaje = "Automovil actualizado correctamente" });
        }
    }
}