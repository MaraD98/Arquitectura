using Application.UseCases.Automovil.Commands.CreateAutomovil;
using Application.UseCases.Automovil.Commands.DeleteAutomovil;
using Application.UseCases.Automovil.Commands.UpdateAutomovil;
using Application.UseCases.Automovil.Queries;
using Application.UseCases.Automovil.Queries.GetAllAutomovil; // Agregado para GetAll
using Application.UseCases.Automovil.Queries.GetAutomovilByChasis; // Agregado para GetByChasis
using Application.UseCases.Automovil.Queries.GetAutomovilById; // Agregado para GetById
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")] // Ruta base: /api/v1/Automovil
    public class AutomovilController : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus;

        public AutomovilController(ICommandQueryBus commandQueryBus)
        {
            _commandQueryBus = commandQueryBus;
        }

        // 1. CREATE (POST /api/v1/Automovil)
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAutomovilCommand command)
        {
            if (command is null) return BadRequest();

            // El Handler devuelve AutomovilQueryResult
            var automovilCreado = await _commandQueryBus.Send(command);

            // Retorna 201 Created con la ubicación del nuevo recurso y el objeto creado
            return CreatedAtAction(nameof(GetById), new { id = automovilCreado.Id }, automovilCreado);
        }

        // 2. READ BY ID (GET /api/v1/Automovil/{id})
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AutomovilQueryResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAutomovilByIdQuery { Id = id };
            var automovil = await _commandQueryBus.Send(query);
            if (automovil == null) return NotFound();
            return Ok(automovil);
        }

        // 3. READ BY CHASSIS (GET /api/v1/Automovil/chasis/{numeroChasis})
        [HttpGet("chasis/{numeroChasis}")]
        [ProducesResponseType(typeof(AutomovilQueryResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByChasis(string numeroChasis)
        {
            var query = new GetAutomovilByChasisQuery { NumeroChasis = numeroChasis };
            var automovil = await _commandQueryBus.Send(query);
            if (automovil == null) return NotFound();
            return Ok(automovil);
        }

        // 4. READ ALL (GET /api/v1/Automovil)
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AutomovilQueryResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            // Usando la query que definimos antes
            var query = new GetAllAutomovilQuery();
            var automoviles = await _commandQueryBus.Send(query);
            return Ok(automoviles);
        }

        // 5. UPDATE (PUT /api/v1/Automovil/{id})
        // NOTA: Se corrigió la ruta y se usa {id:int}
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Si el handler lanza EntityNotFound
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAutomovilCommand command)
        {
            if (command is null || id <= 0) return BadRequest("ID o cuerpo de la solicitud inválido.");

            // Aseguramos que el ID de la ruta se asigne al Command
            command.Id = id;

            // El Handler debe retornar el objeto actualizado o lanzar una excepción si falla.
            var success = await _commandQueryBus.Send(command);

            if (success)
            {
                // Si el handler es exitoso, retornamos 200 OK
                return Ok(new { Message = "Automóvil actualizado con éxito.", UpdatedId = id });
            }

            // Esto se manejaría si el handler devuelve 'false' explícitamente por una regla de negocio
            return BadRequest("Fallo al actualizar el automóvil. Revise los datos.");
        }

        // 6. DELETE (DELETE /api/v1/Automovil/{id})
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("ID inválido.");

            // El handler debe lanzar una excepción si el ID no existe (404 Not Found)
            await _commandQueryBus.Send(new DeleteAutomovilCommand { AutomovilId = id });

            // 204 No Content indica que la acción fue exitosa pero no hay contenido que devolver
            return NoContent();
        }
    }
}