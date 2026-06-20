using MediatR;

namespace BookService.Application.Loans.Commands
{
    public record ReturnLoansCommand : IRequest<bool>
    {
       public int LoanId { get; set; }
    }
}