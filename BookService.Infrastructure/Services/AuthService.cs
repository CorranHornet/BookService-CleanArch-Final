using BookService.Application.Common.Settings;
using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookService.Infrastructure.Services
{
    // Handles authentication-related operations such as user registration, login, and JWT generation.
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService(ApplicationDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        // Registers a new user if the username is not already taken.
        // Passwords are securely hashed before storage.
        public async Task<User?> RegisterAsync(LoginRequestDTO request)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }

            // Check if user already exists
            var userExists = await _context.Users
                .AnyAsync(u => u.Username == request.Username);

            if (userExists)
                return null;

            var user = new User
            {
                Username = request.Username,
                Email = string.Empty,

                // Default role assigned at registration.
                Role = "User"
            };

            // Hash password before saving to database
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // Validates user credentials and returns a JWT token if authentication succeeds.
        public async Task<string?> LoginAsync(LoginRequestDTO request)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }

            // Retrieve user from database
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return null;

            // Verify password using ASP.NET Identity hasher
            var passwordHasher = new PasswordHasher<User>();
            var verificationResult =
                passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
                return null;

            // Generate JWT token upon successful authentication
            return GenerateJwtToken(user);
        }

        // Generates a signed JWT token containing user identity and role claims.
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var keyBytes = Convert.FromBase64String(_jwtSettings.Token.Trim());
            var key = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // Retrieves a user by username.
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}