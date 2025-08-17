using Application.UseCases.DummyEntity.Commands.CreateDummyEntity;
using Application.UseCases.DummyEntity.Commands.DeleteDummyEntity;
using Application.UseCases.DummyEntity.Commands.UpdateDummyEntity;
using Application.UseCases.DummyEntity.Queries.GetAllDummyEntities;
using Application.UseCases.DummyEntity.Queries.GetDummyEntityBy;
using Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    /// <summary>
    /// Ejemplo de controlador que contiene los endpoints que manejan
    /// el CRUD de la entidad Dummy
    /// Todo controlador debe heredar de <see cref="BaseController"/>
    /// ya que el mismo proporciona una funcionalidad para estandarizar
    /// la respuesta de la Api Rest
    /// </summary>
    [ApiController]
    public class DummyEntityController(ICommandQueryBus commandQueryBus) : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));

        [HttpGet("api/v1/[Controller]")]
        public async Task<IActionResult> GetAll(uint pageIndex = 1, uint pageSize = 10)
        {
            var entities = await _commandQueryBus.Send(new GetAllDummyEntitiesQuery() { PageIndex = pageIndex, PageSize = pageSize });

            return Ok(entities);
        }

        [HttpGet("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var entity = await _commandQueryBus.Send(new GetDummyEntityByQuery { DummyIdProperty = id });

            return Ok(entity);
        }

        [HttpPost("api/v1/[Controller]")]
        public async Task<IActionResult> Create(CreateDummyEntityCommand command)
        {
            if (command is null) return BadRequest();

            var id = await _commandQueryBus.Send(command);

            return Created($"api/[Controller]/{id}", new { Id = id });
        }

        [HttpPut("api/v1/[Controller]")]
        public async Task<IActionResult> Update(UpdateDummyEntityCommand command)
        {
            if (command is null) return BadRequest();

            await _commandQueryBus.Send(command);

            return NoContent();
        }

        [HttpDelete("api/v1/[Controller]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            await _commandQueryBus.Send(new DeleteDummyEntityCommand { DummyIdProperty = id });

            return NoContent();
        }
    }
}
