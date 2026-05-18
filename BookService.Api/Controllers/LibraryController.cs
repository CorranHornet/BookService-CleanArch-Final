using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Application.Loans.Commands;
using BookService.Application.MediaItems.Queries;
using BookService.Application.MediaUnits.Handlers;
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

    // GET: books (public)
    [HttpGet("books")]
    public async Task<IActionResult> GetAllBooks(string? search)
    {
            var result = await _mediator.Send(new GetAllMediaItemsQuery(search));
            return Ok(result);
    }

    // POST: books (admin)
    [Authorize(Roles = "Admin")]
    [HttpPost("books")]
    public async Task<IActionResult> AddBook(MediaItemCreateDTO command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }




    // Borrow book (authenticated user
    [Authorize]
    [HttpPost("borrow/{unitId}")]
    public async Task<IActionResult> Borrow(int unitId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

            var result = await _mediator.Send(new CreateLoanCommand
            {
                UserId = int.Parse(userId),
                MediaUnitId = unitId
            });
            return Ok(result);
        }
    }
}