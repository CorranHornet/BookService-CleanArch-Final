using BookService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _service;
        public LoansController(ILoanService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var loans = await _service.GetAllAsync();
            return Ok(loans);
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow(int userId, int mediaUnitId)
        {
            var success = await _service.BorrowAsync(userId, mediaUnitId);

            if (!success)
                return BadRequest(new { message = "MediaUnit is already loaned or invalid." });

            return Ok("MediaUnit borrowed successfully.");
        }

        [HttpPost("return/{loanId}")]
        public async Task<IActionResult> Return(int loanId)
        {
            var result = await _service.ReturnAsync(loanId);

            if (!result)
                return BadRequest(new
                {
                    message = "Loan already returned or invalid.",
                    status = 400
                });

            return Ok();
        }
    }
}

