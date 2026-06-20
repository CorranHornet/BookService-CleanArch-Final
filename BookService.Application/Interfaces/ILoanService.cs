using BookService.Application.DTOs;

namespace BookService.Application.Interfaces
{
    public interface ILoanService
    {
        Task<bool> BorrowAsync(int userId, int mediaUnitId);
        Task<bool> ReturnAsync(int loanId);
        Task<IEnumerable<LoanResponseDTO>> GetAllAsync();
    }
}
