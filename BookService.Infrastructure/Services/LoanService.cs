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

        // 2. We can use Mapster to create the initial tracking state if desired, 
        // but since it's just plain IDs from parameters, using Mapster's dynamic parameter mapping 
        // or a simple object initialization keeps it readable. Let's use Mapster to stay consistent!

        var loan = new { UserId = userId, MediaUnitId = mediaUnitId }.Adapt<Loan>();
        loan.LoanDate = DateTime.UtcNow; // Set our tracking timestamp
        
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


        // 3. Collection Pattern: Mapster automatically travels down into the nested 
        // User and MediaUnit navigation properties and converts them into UserDTO and MediaUnitDTO!
        return loans.Adapt<IEnumerable<LoanResponseDTO>>();
    }
}