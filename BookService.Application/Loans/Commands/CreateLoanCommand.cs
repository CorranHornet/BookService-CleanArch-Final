using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.Loans.Commands
{
    public record CreateLoanCommand : IRequest<LoanResponseDTO>
    {
        public int UserId { get; set; }
        public int MediaUnitId { get; set; }
    }
}
