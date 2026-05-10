using BookService.Api.DTOs;
using BookService.Domain.Entities;
namespace BookService.Api.Services
{
    public interface ILoanService
    {
        Task<bool> BorrowAsync(int userId, int mediaUnitId);
        Task<bool> ReturnAsync(int loanId);
        Task<IEnumerable<LoanResponseDTO>> GetAllAsync();
    }
}
