using Application.UseCases.Automovil.Commands.CreateAutomovil;
using Application.UseCases.Automovil.Commands.DeleteAutomovil;
using Application.UseCases.Automovil.Queries.BuscarAutomovil;
using Application.UseCases.Automovil.Queries.BuscarAutomovilPorChasis;
using Application.UseCases.DummyEntity.Commands.DeleteDummyEntity;
using Application.UseCases.DummyEntity.Queries.BuscarAutomovilQuery;
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

            return Created($"api/v1/[controller]/{id}", new { Id = id });
        }

        [HttpDelete("api/v1/[controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("ID inválido.");

            await _commandQueryBus.Send(new DeleteAutomovilCommand { AutomovilId = id });

            return NoContent();
        }

        [HttpGet("api/v1/[Controller]")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
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

    }
}