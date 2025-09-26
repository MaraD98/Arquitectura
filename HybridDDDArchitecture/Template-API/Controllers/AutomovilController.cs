using Application.UseCases.Automovil.Commands.CreateAutomovil;
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
    }
}