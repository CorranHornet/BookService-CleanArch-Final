using BookService.Application.DTOs;
using BookService.Application.Loans.Commands;
using BookService.Application.Loans.Queries;
using Mapster;
using MediatR;

namespace BookService.Application.Loans.Handlers
{
    internal class GetLoansQueryHandler
        : IRequestHandler<GetLoansQuery, IEnumerable<LoanResponseDTO>>
    {
        private readonly ILoanRepository _repo;


        public GetLoansQueryHandler(
            ILoanRepository repo)

        {
            _repo = repo;

        }

        
public async Task<IEnumerable<LoanResponseDTO>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
        {
            var loans = await _repo.GetAllWithIncludes();

            return loans.Adapt<IEnumerable<LoanResponseDTO>>();
        }
    }
}

