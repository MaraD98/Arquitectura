using Application.UseCases.Automovil.Commands.CreateAutomovil;
using Application.UseCases.Automovil.Commands.DeleteAutomovil;
<<<<<<< HEAD
using Application.UseCases.Automovil.Queries.BuscarAutomovil;
using Application.UseCases.Automovil.Queries.BuscarAutomovilPorChasis;
using Application.UseCases.DummyEntity.Commands.DeleteDummyEntity;
using Application.UseCases.DummyEntity.Queries.BuscarAutomovilQuery;
=======
using Application.UseCases.Automovil.Commands.UpdateAutomovil;
using Application.UseCases.Automovil.Queries.GetAutomovilById;
using Application.UseCases.Automovil.Queries.GetAutomovilByChasis;
using Application.UseCases.Automovil.Queries.GetAllAutomovil;
>>>>>>> origin/Mara
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

<<<<<<< HEAD
            return Created($"api/v1/[controller]/{id}", new { Id = id });
=======
            return Created($"api/v1/[controller]/{id}", new
            {
                Id = id,
                mensaje = "Automovil creado correctamente"
            });
>>>>>>> origin/Mara
        }

        [HttpDelete("api/v1/[controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("ID inválido.");

            await _commandQueryBus.Send(new DeleteAutomovilCommand { AutomovilId = id });

<<<<<<< HEAD
            return NoContent();
=======
            return Ok(new { mensaje = "Automovil eliminado correctamente" });
        }

        [HttpPut("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAutomovilCommand command)
        {
            await _commandQueryBus.Send(new UpdateAutomovilWrapper(id, command));

            return Ok(new { mensaje = "Automovil actualizado correctamente" });
        }

        [HttpGet("api/v1/[Controller]/{id}")] 
        public async Task<IActionResult> GetById(int id)
        {

            if (id <= 0) return BadRequest("ID inválido.");

            var automovil = await _commandQueryBus.Send(new GetAutomovilByIdQuery { Id = id });

            return Ok(automovil);
        }

        [HttpGet("api/v1/[Controller]chasis/{numeroChasis}")]  
        public async Task<IActionResult> GetByNumeroChasis(string numeroChasis)
        {
            if (string.IsNullOrEmpty(numeroChasis)) return BadRequest();

            var automovil = await _commandQueryBus.Send(new GetAutomovilByChasisQuery { NumeroChasis = numeroChasis });

            return Ok(automovil);
>>>>>>> origin/Mara
        }

        [HttpGet("api/v1/[Controller]")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
<<<<<<< HEAD
            var entities = await _commandQueryBus.Send(new BuscarAutomovilQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }

                [HttpGet("api/v1/[Controller]{Chasis}/Chasis")]
        public async Task<IActionResult> GetByChasis(string Chasis)
        {
            if (string.IsNullOrEmpty(Chasis)) return BadRequest();

            var entity = await _commandQueryBus.Send(new BuscarAutomovilPorChasisQuery { Chasis = Chasis });

            return Ok(entity);
        }

=======
            var entities = await _commandQueryBus.Send(new GetAllAutomovilQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }
>>>>>>> origin/Mara
    }
}