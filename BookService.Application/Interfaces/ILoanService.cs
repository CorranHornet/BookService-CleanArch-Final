using BookService.Application.DTOs;
using BookService.Domain.Entities;
namespace BookService.Application.Interfaces
{
    public interface ILoanService
    {
        Task<bool> BorrowAsync(int userId, int mediaUnitId);
        Task<bool> ReturnAsync(int loanId);
        Task<IEnumerable<LoanResponseDTO>> GetAllAsync();
    }
}
