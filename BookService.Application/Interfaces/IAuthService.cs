using BookService.Domain.Entities;
using BookService.Application.DTOs;


namespace BookService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(LoginRequestDTO request);
        Task<string?> LoginAsync(LoginRequestDTO request);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}