using BookService.Application.DTOs;
using BookService.Api.Services;
using BookService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(LoginRequestDTO request)
        {
            var user = await _authService.RegisterAsync(request);

            if (user == null)
                return BadRequest("Username already exists or invalid input.");

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Role
            });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginRequestDTO request)
        {
            var token = await _authService.LoginAsync(request);
            if (token is null)
                return Unauthorized("Invalid username or password.");

            var user = await _authService.GetUserByUsernameAsync(request.Username);

            return Ok(new LoginResponseDTO
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }




    }
}
