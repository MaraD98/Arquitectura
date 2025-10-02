using Application.UseCases.Automovil.Commands.CreateAutomovil;
using Application.UseCases.Automovil.Commands.DeleteAutomovil;
using Application.UseCases.Automovil.Commands.UpdateAutomovil;
using Application.UseCases.Automovil.Queries;
using Application.DataTransferObjects; // Asumo que AutomovilDto está aquí
using Core.Application.ComandQueryBus.Buses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace Template_API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AutomovilController : ControllerBase
    {
        private readonly ICommandQueryBus _commandQueryBus;

        public AutomovilController(ICommandQueryBus commandQueryBus)
        {
            _commandQueryBus = commandQueryBus;
        }

        // REQUISITO 1: POST /api/v1/automovil
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAutomovilCommand command)
        {
            if (command is null) return BadRequest();

            var automovilCreado = await _commandQueryBus.Send<AutomovilDto>(command);

            return CreatedAtAction(nameof(GetById), new { id = automovilCreado.Id }, automovilCreado);
        }

        // REQUISITO 2: PUT /api/v1/automovil/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAutomovilCommand command)
        {
            if (command is null || id <= 0) return BadRequest("ID o cuerpo de la solicitud inválido.");

            command.Id = id;

            // El handler devuelve 'bool'
            await _commandQueryBus.Send<bool>(command);

            return Ok(new { Message = "Automóvil actualizado con éxito.", UpdatedId = id });
        }

        // REQUISITO 3: DELETE /api/v1/automovil/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("ID inválido.");

            // Se envía el comando, el Handler se encarga de la lógica de negocio y excepciones
            await _commandQueryBus.Send(new DeleteAutomovilCommand { AutomovilId = id });

            return NoContent(); // Status 204 No Content
        }

        // REQUISITO 4: GET /api/v1/automovil/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AutomovilDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAutomovilByIdQuery { Id = id };
            var automovil = await _commandQueryBus.Send<AutomovilDto>(query);

            return Ok(automovil);
        }

        // REQUISITO 5: GET /api/v1/automovil/chasis/{numeroChasis}
        [HttpGet("chasis/{numeroChasis}")]
        [ProducesResponseType(typeof(AutomovilDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByChasis(string numeroChasis)
        {
            var query = new GetAutomovilByChasisQuery { NumeroChasis = numeroChasis };
            var automovil = await _commandQueryBus.Send<AutomovilDto>(query);

            return Ok(automovil);
        }

        // REQUISITO 6: GET /api/v1/automovil
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AutomovilDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllAutomovilesQuery();
            var automoviles = await _commandQueryBus.Send<IEnumerable<AutomovilDto>>(query);

            return Ok(automoviles);
        }
    }
}