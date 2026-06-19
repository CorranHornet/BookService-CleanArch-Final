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

            // 2. Use Mapster to create a clean UserDTO instead of an anonymous object
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

            // Fix: Explicitly handle the case where the user might not be found
            if (user is null)
                return NotFound( new { message = "User not found after successful authentication." });

            // 3. Map the User entity to LoginResponseDTO, then assign the Token field
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
