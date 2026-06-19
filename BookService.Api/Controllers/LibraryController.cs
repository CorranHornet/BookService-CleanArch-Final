using BookService.Application.Loans.Commands;
using BookService.Application.Loans.Queries;
using BookService.Application.MediaItems.Commands;
using BookService.Application.MediaItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LibraryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Returns all books (public endpoint)
        [HttpGet("books")]
        public async Task<IActionResult> GetAllBooks(string? search)
        {
            var result = await _mediator.Send(new GetAllMediaItemsQuery(search));
            return Ok(result);
        }

        // Creates a new book (admin only)
        // Uses CQRS command instead of passing DTO directly
        [Authorize(Roles = "Admin")]
        [HttpPost("books")]
        public async Task<IActionResult> CreateBook(CreateMediaItemCommand command)
        {
            var result = await _mediator.Send(command);

            // Returns 201 Created for proper REST semantics
            return CreatedAtAction(nameof(GetAllBooks), new { id = result.Id }, result);
        }

        // Allows an authenticated user to borrow a book unit
        [Authorize]
        [HttpPost("borrow/{unitId}")]
        public async Task<IActionResult> BorrowBook(int unitId)
        {
            // Extract user id from JWT token claims
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Unauthorized("User identity not found in token.");

            var command = new CreateLoanCommand
            {
                UserId = int.Parse(userIdClaim),
                MediaUnitId = unitId
            };

            var result = await _mediator.Send(command);

            // Return successful loan creation
            return Ok(result);
        }

        // Optional endpoint: returns current user's loans (example of extensibility)
        [Authorize]
        [HttpGet("my-loans")]
        public async Task<IActionResult> GetMyLoans()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Unauthorized();

            // Example query - assumes you have this query in Application layer
            var result = await _mediator.Send(new GetLoansQuery());

            return Ok(result);
        }
    }
}