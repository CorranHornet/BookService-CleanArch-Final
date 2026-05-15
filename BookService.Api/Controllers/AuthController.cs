using BookService.Application.DTOs;
using BookService.Infrastructure.Services;
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
        public async Task<ActionResult<User>> Register(LoginRequestDTO request)
        {
            var user = await _authService.RegisterAsync(request);

            if (user == null)
                return BadRequest("Username already exists or invalid input.");

            // 2. Use Mapster to create a clean UserDTO instead of an anonymous object
            return Ok(user.Adapt<UserDTO>());
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginRequestDTO request)
        {
            var token = await _authService.LoginAsync(request);
            if (token is null)
                return Unauthorized("Invalid username or password.");

            var user = await _authService.GetUserByUsernameAsync(request.Username);

            // 3. Map the User entity to LoginResponseDTO, then assign the Token field
            var response = user.Adapt<LoginResponseDTO>();
            response.Token = token;

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }




    }
}
