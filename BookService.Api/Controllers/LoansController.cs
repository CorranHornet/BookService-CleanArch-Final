using BookService.Application.Loans.Commands;
using BookService.Application.Loans.Queries;
using MediatR;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetLoansQuery());
            return Ok(result);
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow(int userId, int mediaUnitId)
        {
            var result = await _mediator.Send( new CreateLoanCommand
            {
                UserId = userId,
                MediaUnitId = mediaUnitId
            });

            return Ok(result);
        }

        [HttpPost("return/{loanId}")]
        public async Task<IActionResult> Return(int loanId)
        {
            var result = await _mediator.Send(new { message = "Loan already returned or invalid."});

            return Ok();
        }
    }
}

