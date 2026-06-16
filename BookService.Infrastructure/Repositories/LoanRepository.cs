using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookService.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> UserExists(int userId)
            => _context.Users.AnyAsync(u => u.Id == userId);

        public Task<bool> MediaUnitExists(int mediaUnitId)
            => _context.MediaUnits.AnyAsync(mu => mu.Id == mediaUnitId);

        public Task<bool> IsAlreadyLoaned(int mediaUnitId)
            => _context.Loans.AnyAsync(l => l.MediaUnitId == mediaUnitId && l.ReturnDate == null);

        public async Task AddLoan(Loan loan)
            => await _context.Loans.AddAsync(loan);

        public Task<Loan?> GetById(int loanId)
            => _context.Loans.FirstOrDefaultAsync(l => l.Id == loanId);

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();

        public Task<List<Loan>> GetAllWithIncludes()
            => _context.Loans
                .Include(l => l.User)
                .Include(l => l.MediaUnit)
                .ToListAsync();
    }
}