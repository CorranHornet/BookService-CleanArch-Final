using BookService.Domain.Entities;

namespace BookService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> HasActiveLoans(int userId);
        Task SaveChangesAsync();
    }
}