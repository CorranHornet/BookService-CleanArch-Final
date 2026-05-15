using BookService.Application.Loans.Commands;
using MediatR;

namespace BookService.Application.Loans.Handlers
{
    internal class ReturnLoanCommandHandler : IRequestHandler<ReturnLoansCommand, bool>
    {
        private readonly ILoanRepository _repo;


        public ReturnLoanCommandHandler(
            ILoanRepository repo)

        {
            _repo = repo;

        }

        public async Task<bool> Handle(ReturnLoansCommand request, CancellationToken cancellationToken)
        {
            var loan = await _repo.GetById(request.LoanId);
            if (loan == null || loan.ReturnDate != null)
                return false;

            loan.ReturnDate = DateTime.UtcNow;

            await _repo.SaveChanges();
            return true;
        }
    }
}