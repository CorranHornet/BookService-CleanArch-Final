using BookService.Application.Users.Commands;
using BookService.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Returns all users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetUsersQuery());
            return Ok(result);
        }

        // Returns a single user by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // Creates a new user
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            var user = await _mediator.Send(command);

            if (user == null)
                return BadRequest("User could not be created.");

            // REST-correct response for creation
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // Deletes a user
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}