
using BookService.Application.Interfaces;
using BookService.Application.Users.Commands;
using BookService.Application.Users.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _mediator.Send(new GetUsersQuery()));
            
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));
            return user == null ? NotFound() : Ok(user);
            
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return result ? NoContent() : NotFound();
              
            
        }
    }
}
