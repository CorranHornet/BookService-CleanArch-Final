using BookService.Infrastructure.Persistence;
using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;


using Microsoft.EntityFrameworkCore;
using Mapster;

namespace BookService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllAsync()
        {
            // 2. Used ProjectToType to automatically build the clean SQL SELECT statement
            return await _context.Users
                .ProjectToType<UserResponseDTO>()
                .ToListAsync();
                
        }

        public async Task<UserResponseDTO?> GetByIdAsync(int id)
        {
            // 3. Cleaned up the manual projection loop block
            return await _context.Users
                .Where(u => u.Id == id)
                .ProjectToType<UserResponseDTO>()
                .FirstOrDefaultAsync();
        }

        public async Task<UserResponseDTO> CreateAsync(UserCreateDTO dto)
        {
            // 4. Mapster instantiates the entity and pulls the class defaults 
            // (PasswordHash = string.Empty and Role = "User") automatically!
            var user = dto.Adapt<User>();
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 5. Converts tracked entity back to the presentation DTO
            return user.Adapt<UserResponseDTO>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Loans)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return false;

            var hasActiveLoans = user.Loans.Any(l => l.ReturnDate == null);

            if (hasActiveLoans)
                throw new Exception("Cannot delete user with active loans.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}