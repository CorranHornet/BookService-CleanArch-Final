using BookService.Application.Genres.Commands;
using BookService.Application.Genres.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Api.Controllers
{
    [Authorize]
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/genre
        [HttpGet]
        public async Task<IActionResult> GetAll()

        => Ok(await _mediator.Send(new GetGenresQuery()));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _mediator.Send(new GetGenreByIdQuery(id)));

         // POST: api/genre
        [HttpPost]
        public async Task<IActionResult> Create(CreateGenreCommand command)
             => Ok(await _mediator.Send(command));

        // DELETE: api/genre/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)

           => Ok(await _mediator.Send(new DeleteGenreCommand(id)));
    }
}
