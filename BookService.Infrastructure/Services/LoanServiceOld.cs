using BookService.Infrastructure.Persistence;
using BookService.Application.Interfaces;
using BookService.Application.DTOs;
using BookService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace BookService.Infrastructure.Services.TST
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

            // 2. Map structural values using standard mapping consistency

            var loan = new { UserId = userId, MediaUnitId = mediaUnitId }.Adapt<Loan>();
            loan.LoanDate = DateTime.UtcNow;
            loan.ReturnDate = null;

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
            // 3. ProjectToType tells Entity Framework to only pull the specific columns 
            // required by LoanResponseDTO, UserDTO, and MediaUnitDTO out of the SQL Database.
            // Notice you no longer even need the .Include() statements, Mapster builds the joins for you!
            return await _context.Loans
                .ProjectToType<LoanResponseDTO>()
                .ToListAsync();
        }
    }
}