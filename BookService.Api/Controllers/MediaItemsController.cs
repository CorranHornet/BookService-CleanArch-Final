
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

       
        [HttpGet]
        public async Task<IActionResult> GetAll(string? search)
        {
            var result = await _mediator.Send(new GetAllMediaItemsQuery(search));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMediaItemCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, UpdateMediaItemCommand command)
        {
            command.Id = id;

            var success = await _mediator.Send(command);

            if (!success)
                return NotFound();

            return NoContent();
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetMediaItemByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
         
       



        
        

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteMediaItemCommand(id));

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}