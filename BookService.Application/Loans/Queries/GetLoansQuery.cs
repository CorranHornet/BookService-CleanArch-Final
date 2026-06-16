using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.Loans.Queries
{
    public record GetLoansQuery : IRequest<IEnumerable<LoanResponseDTO>>
    {
    }
}
