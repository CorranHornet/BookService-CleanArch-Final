using BookService.Infrastructure.Persistence;
using BookService.Api.DTOs;


using BookService.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace BookService.Api.Services
{
    public class LoanService : ILoanService
    {
        private readonly ApplicationDbContext _context;

        public LoanService(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------------
        // BORROW (SAFE + CORRECT)
        // -------------------------
        public async Task<bool> BorrowAsync(int userId, int mediaUnitId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists) return false;

            var unitExists = await _context.MediaUnits.AnyAsync(mu => mu.Id == mediaUnitId);
            if (!unitExists) return false;

            // 🔥 ONLY ONE ACTIVE LOAN ALLOWED
            var isAlreadyLoaned = await _context.Loans
                .AnyAsync(l => l.MediaUnitId == mediaUnitId && l.ReturnDate == null);

            if (isAlreadyLoaned)
                return false;

            var loan = new Loan
            {
                UserId = userId,
                MediaUnitId = mediaUnitId,
                LoanDate = DateTime.UtcNow,
                ReturnDate = null
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return true;
        }

        // -------------------------
        // RETURN
        // -------------------------
        public async Task<bool> ReturnAsync(int loanId)
        {
            var loan = await _context.Loans
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null) return false;
            if (loan.ReturnDate != null) return false;

            loan.ReturnDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // -------------------------
        // GET ALL
        // -------------------------
        public async Task<IEnumerable<LoanResponseDTO>> GetAllAsync()
        {
            return await _context.Loans
                .Include(l => l.User)
                .Include(l => l.MediaUnit)
                .Select(l => new LoanResponseDTO
                {
                    Id = l.Id,
                    LoanDate = l.LoanDate,
                    ReturnDate = l.ReturnDate,

                    User = new UserDTO
                    {
                        Id = l.User.Id,
                        Username = l.User.Username,
                        Email = l.User.Email
                    },

                    MediaUnit = new MediaUnitDTO
                    {
                        Id = l.MediaUnit.Id,
                        Title = l.MediaUnit.Title,
                        Number = l.MediaUnit.Number,
                        MediaItemId = l.MediaUnit.MediaItemId
                    }
                })
                .ToListAsync();
        }
    }
}