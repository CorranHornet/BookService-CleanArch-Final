using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<User>> GetAllAsync()
            => _context.Users.ToListAsync();

        public Task<User?> GetByIdAsync(int id)
            => _context.Users
                .Include(u => u.Loans)
                .FirstOrDefaultAsync(u => u.Id == id);

        public Task AddAsync(User user)
            => _context.Users.AddAsync(user).AsTask();

        public Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            return Task.CompletedTask;
        }

        public Task<bool> HasActiveLoans(int userId)
            => _context.Loans.AnyAsync(l => l.UserId == userId && l.ReturnDate == null);

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}