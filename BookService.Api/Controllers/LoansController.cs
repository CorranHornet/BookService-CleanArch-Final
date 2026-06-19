using BookService.Application.Loans.Commands;
using BookService.Application.Loans.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Returns all loans in the system
        // Typically restricted to Admin in production systems
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetLoansQuery());
            return Ok(result);
        }

        // Creates a new loan (borrow a book/media item)
        // User identity should be handled inside the command/handler via JWT claims
        [Authorize]
        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow([FromBody] CreateLoanCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // Marks a loan as returned
        // Uses loanId from route and delegates logic to CQRS handler
        [Authorize]
        [HttpPost("return/{loanId}")]
        public async Task<IActionResult> Return(int loanId)
        {
            var command = new ReturnLoansCommand
            {
                LoanId = loanId
            };

            var result = await _mediator.Send(command);

            // Returns 200 OK if successful, otherwise 400 BadRequest
            return result
                ? Ok(result)
                : BadRequest(new { message = "Invalid or already returned loan." });
        }
    }
}