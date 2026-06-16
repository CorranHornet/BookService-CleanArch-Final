using BookService.Domain.Entities;

namespace BookService.Application.Interfaces
{
    public interface ILoanRepository
    {
        Task<bool> UserExists(int userId);
        Task<bool> MediaUnitExists(int mediaUnitId);
        Task<bool> IsAlreadyLoaned(int mediaUnitId);
        Task AddLoan(Loan loan);
        Task<Loan?> GetById(int loanId);
        Task SaveChangesAsync();
        Task<List<Loan>> GetAllWithIncludes();
    }
}