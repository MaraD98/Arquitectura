using Application.UseCases.Automovil.Commands;
using Application.UseCases.Automovil.Commands.CreateAutomovil;
using Application.UseCases.Automovil.Commands.DeleteAutomovil;
using Application.UseCases.Automovil.Commands.UpdateAutomovil;
using Application.UseCases.Automovil.Queries;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AutomovilController : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus;

        public AutomovilController(ICommandQueryBus commandQueryBus)
        {
            _commandQueryBus = commandQueryBus;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAutomovilCommand command)
        {
            if (command is null) return BadRequest();
            var id = await _commandQueryBus.Send(command);
            return Created($"api/v1/automovil/{id}", new { Id = id });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAutomovilByIdQuery { Id = id };
            var automovil = await _commandQueryBus.Send(query);
            if (automovil == null) return NotFound();
            return Ok(automovil);
        }

        [HttpGet("chasis/{numeroChasis}")]
        public async Task<IActionResult> GetByChasis(string numeroChasis)
        {
            var query = new GetAutomovilByChasisQuery { NumeroChasis = numeroChasis };
            var automovil = await _commandQueryBus.Send(query);
            if (automovil == null) return NotFound();
            return Ok(automovil);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllAutomovilesQuery();
            var automoviles = await _commandQueryBus.Send(query);
            return Ok(automoviles);
        }

        [HttpPut("api/v1/[controller]/{id}")]
        public async Task<IActionResult> Update(int id, UpdateAutomovilCommand command)
        {
            if (command is null || id != command.Id) return BadRequest();

            // Necesitas asegurarte de que el ID de la ruta se asigne al Command
            command.Id = id;

            // El Handler retorna un 'bool', indicando éxito o fracaso
            var success = await _commandQueryBus.Send(command);

            if (success)
            {
                // 200 OK es la respuesta estándar para PUT cuando se retorna un objeto o un éxito simple
                return Ok(command);
            }

            // Si el handler lanza una EntityDoesNotExistException, el middleware de excepciones la manejará.
            // Si retorna 'false' (p. ej. por una validación que no pasó), sería un BadRequest.
            return BadRequest("Fallo al actualizar el automóvil.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("ID inválido.");
            await _commandQueryBus.Send(new DeleteAutomovilCommand { AutomovilId = id });
            return NoContent();
        }
    }
}