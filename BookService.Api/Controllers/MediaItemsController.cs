using BookService.Application.MediaItems.Commands;
using BookService.Application.MediaItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MediaItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Retrieves all media items.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var result = await _mediator.Send(new GetAllMediaItemsQuery(search));
            return Ok(result);
        }

        // Retrieves a media item by id.
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetMediaItemByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Creates a new media item.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateMediaItemCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        // Updates an existing media item.
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateMediaItemCommand command)
        {
            command.Id = id;

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound();

            return NoContent();
        }

        // Deletes a media item.
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteMediaItemCommand(id));

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
        
        
