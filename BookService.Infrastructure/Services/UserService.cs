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
            return await _context.Users
                .ProjectToType<UserResponseDTO>()
                .ToListAsync();
        }

        public async Task<UserResponseDTO?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .ProjectToType<UserResponseDTO>()
                .FirstOrDefaultAsync();
        }

        public async Task<UserResponseDTO> CreateAsync(UserCreateDTO dto)
        {
            var user = dto.Adapt<User>();
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

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
