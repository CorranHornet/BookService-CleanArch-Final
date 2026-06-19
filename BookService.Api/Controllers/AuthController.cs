using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mapster;

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
        [AllowAnonymous]
        public async Task<ActionResult<User>> Register(LoginRequestDTO request)
        {
            var user = await _authService.RegisterAsync(request);

            if (user == null)
                return BadRequest(new { message = "Username already exists or invalid input." });

            // Return a DTO instead of exposing the domain entity directly
            return Ok(user.Adapt<UserDTO>());
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO request)
        {
            var token = await _authService.LoginAsync(request);
            if (token is null)
                return Unauthorized(new { message = "Invalid username or password." });

            var user = await _authService.GetUserByUsernameAsync(request.Username);

            // Ensure user still exists after successful authentication
            if (user is null)
                return NotFound( new { message = "User not found after successful authentication." });

            // Map user data to response DTO and attach generated JWT token
            var response = user.Adapt<LoginResponseDTO>();
            response.Token = token;

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok (new { message = "You are authenticated!" });
        }
    }
}

