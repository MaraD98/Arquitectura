using Application.UseCases.Automovil.Commands.CreateAutomovil;
using Application.UseCases.Automovil.Commands.DeleteAutomovil;
using Application.UseCases.Automovil.Commands.UpdateAutomovil;
using Application.DataTransferObjects;
using Core.Application.ComandQueryBus.Buses; // Se asume que este using es correcto
using Application.UseCases.Automovil.Queries;

using Core.Application;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Template_API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AutomovilController : ControllerBase
    {
        private readonly ICommandQueryBus _commandQueryBus;

        public AutomovilController(ICommandQueryBus commandQueryBus)
        {
            _commandQueryBus = commandQueryBus;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAutomovilCommand command)
        {
            if (command is null) return BadRequest();

            // CORRECCIÓN CS0305: Usamos el método Send con UN solo tipo genérico (TResponse)
            var automovilCreado = await _commandQueryBus.Send<AutomovilDto>(command);

            return CreatedAtAction(nameof(GetById), new { id = automovilCreado.Id }, automovilCreado);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AutomovilDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAutomovilByIdQuery { Id = id };

            // CORRECCIÓN CS0305: Usamos el método Send con UN solo tipo genérico (TResponse)
            var automovil = await _commandQueryBus.Send<AutomovilDto>(query);

            if (automovil == null) return NotFound();
            return Ok(automovil);
        }

        [HttpGet("chasis/{numeroChasis}")]
        [ProducesResponseType(typeof(AutomovilDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByChasis(string numeroChasis)
        {
            var query = new GetAutomovilByChasisQuery { NumeroChasis = numeroChasis };

            // CORRECCIÓN CS0305: Usamos el método Send con UN solo tipo genérico (TResponse)
            var automovil = await _commandQueryBus.Send<AutomovilDto>(query);

            if (automovil == null) return NotFound();
            return Ok(automovil);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AutomovilDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllAutomovilesQuery();

            // CORRECCIÓN CS0305: Usamos el método Send con UN solo tipo genérico (TResponse)
            var automoviles = await _commandQueryBus.Send<IEnumerable<AutomovilDto>>(query);

            return Ok(automoviles);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAutomovilCommand command)
        {
            if (command is null || id <= 0) return BadRequest("ID o cuerpo de la solicitud inválido.");

            command.Id = id;

            // CORRECCIÓN CS0305: Usamos el método Send con UN solo tipo genérico (TResponse), asumo devuelve 'bool' o 'Unit'
            // Si devuelve un valor de éxito/fallo (como bool) o el tipo Unit de MediatR
            var success = await _commandQueryBus.Send<bool>(command);

            if (success)
            {
                return Ok(new { Message = "Automóvil actualizado con éxito.", UpdatedId = id });
            }

            return BadRequest("Fallo al actualizar el automóvil. Revise los datos.");
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("ID inválido.");

            // CORRECCIÓN: Si el comando Delete no devuelve nada, se usa el tipo 'Unit' o 'bool'.
            // Para simplificar, asumiremos que si no devuelve nada, usamos la versión no genérica
            // o el tipo de MediatR que es 'Unit' (si el proyecto tiene una referencia a MediatR).
            // Lo dejamos sin tipo genérico para el comando Delete (como estaba originalmente), asumiendo que el bus lo maneja.
            await _commandQueryBus.Send(new DeleteAutomovilCommand { AutomovilId = id });

            return NoContent();
        }
    }
}