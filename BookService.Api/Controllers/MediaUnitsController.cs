using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookService.Application.MediaUnits.Commands;
using BookService.Application.MediaUnits.Queries;

namespace BookService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaUnitsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MediaUnitsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Retrieves all media units
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllMediaUnitsQuery());
            return Ok(result);
        }

        // Retrieves a single media unit by id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetMediaUnitByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Creates a new media unit
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(CreateMediaUnitCommand command)
        {
            var result = await _mediator.Send(command);

            // Keeps your current contract (no restructuring of handlers required)
            if (result == null)
                return NotFound($"MediaItem {command.MediaItemId} not found.");

            return Ok(result);
        }

        // Updates an existing media unit
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdateMediaUnitCommand command)
        {
            command.Id = id;

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound();

            return NoContent();
        }

        // Deletes a media unit
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteMediaUnitCommand(id));

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}