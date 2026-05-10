using BookService.Domain.Entities;
using BookService.Api.DTOs;


namespace BookService.Api.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(LoginRequestDTO request);
        Task<string> LoginAsync(LoginRequestDTO request);
        Task<User> GetUserByUsernameAsync(string username);
    }
}