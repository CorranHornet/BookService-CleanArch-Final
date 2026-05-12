using BookService.Domain.Entities;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task DeleteAsync(User user);
    Task<bool> HasActiveLoans(int userId);
    Task SaveAsync();
}