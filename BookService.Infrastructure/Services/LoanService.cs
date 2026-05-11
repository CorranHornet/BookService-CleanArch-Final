using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;

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

        var loan = new Loan
        {
            UserId = userId,
            MediaUnitId = mediaUnitId,
            LoanDate = DateTime.UtcNow
        };

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

        return loans.Select(l => new LoanResponseDTO
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
        });
    }
}