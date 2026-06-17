using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Application.Loans.Commands;
using BookService.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace BookService.Application.Loans.Handlers
{
    public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, LoanResponseDTO>
    {
        private readonly ILoanRepository _repo;
        private readonly IMapper _mapper;

        public CreateLoanCommandHandler(ILoanRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<LoanResponseDTO> Handle(CreateLoanCommand request, CancellationToken ct)
        {
            var loan = _mapper.Map<Loan>(request);
            loan.LoanDate = DateTime.UtcNow;

            await _repo.AddLoan(loan);
            await _repo.SaveChangesAsync();

            return _mapper.Map<LoanResponseDTO>(loan);
        }
    }
}