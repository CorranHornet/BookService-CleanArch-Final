using BookService.Application.Loans.Commands;
using BookService.Application.Interfaces;
using MapsterMapper;
using MediatR;

namespace BookService.Application.Loans.Handlers;

public class ReturnLoanCommandHandler : IRequestHandler<ReturnLoansCommand, bool>
{
    private readonly ILoanRepository _repo;
    private readonly IMapper _mapper; 

    public ReturnLoanCommandHandler(ILoanRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<bool> Handle(ReturnLoansCommand request, CancellationToken ct)
    {
        // 1. Fetch the existing entity
        var loan = await _repo.GetById(request.LoanId);

        // 2. Validate
        if (loan == null || loan.ReturnDate != null)
            return false;

        // 3. Update the domain entity
        loan.ReturnDate = DateTime.UtcNow;

        // 4. Persist changes using the consistent SaveChanges()
        await _repo.SaveChangesAsync();

        return true;
    }
}