using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController(IMediaItemService itemService, ILoanService loanService) : ControllerBase
    {
        // Hämta alla böcker - Publikt
        [HttpGet("books")]
        public async Task<IActionResult> GetAllBooks() => Ok(await itemService.GetAllAsync());

        // Lägg till bok - ENDAST ADMIN
        [Authorize(Roles = "Admin")]
        [HttpPost("books")]
        public async Task<IActionResult> AddBook(MediaItemCreateDTO dto)
        {
            var result = await itemService.CreateAsync(dto);
            return Ok(result);
        }

        // Låna en bok - INLOGGAD ANVÄNDARE
        [Authorize]
        [HttpPost("borrow/{unitId}")]
        public async Task<IActionResult> Borrow(int unitId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var success = await loanService.BorrowAsync(int.Parse(userId), unitId);
            return success ? Ok("Boken lånad.") : BadRequest("Boken är inte tillgänglig.");
        }
    }
}