using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using Mapster;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _repo;

    public LoanService(ILoanRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> BorrowAsync(int userId, int mediaUnitId)
    {
        if (!await _repo.UserExists(userId)) return false;
        if (!await _repo.MediaUnitExists(mediaUnitId)) return false;
        if (await _repo.IsAlreadyLoaned(mediaUnitId)) return false;

        var loan = new { UserId = userId, MediaUnitId = mediaUnitId }.Adapt<Loan>();
        loan.LoanDate = DateTime.UtcNow;
        
        await _repo.AddLoan(loan);
        await _repo.SaveChanges();

        return true;
    }

    public async Task<bool> ReturnAsync(int loanId)
    {
        var loan = await _repo.GetById(loanId);
        if (loan == null || loan.ReturnDate != null)
            return false;

        loan.ReturnDate = DateTime.UtcNow;

        await _repo.SaveChanges();
        return true;
    }

    public async Task<IEnumerable<LoanResponseDTO>> GetAllAsync()
    {
        var loans = await _repo.GetAllWithIncludes();
        return loans.Adapt<IEnumerable<LoanResponseDTO>>();
    }
}
