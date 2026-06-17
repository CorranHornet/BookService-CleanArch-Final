using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Application.Loans.Commands;
using BookService.Application.Loans.Queries;
using Mapster;
using MapsterMapper;
using MediatR;

namespace BookService.Application.Loans.Handlers
{
    internal class GetLoansQueryHandler
        : IRequestHandler<GetLoansQuery, IEnumerable<LoanResponseDTO>>
    {
        private readonly ILoanRepository _repo;
        private readonly IMapper _mapper;


        public GetLoansQueryHandler(
            ILoanRepository repo, IMapper mapper)

        {
            _repo = repo;
            _mapper = mapper;

        }

        
public async Task<IEnumerable<LoanResponseDTO>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
        {
            var loans = await _repo.GetAllWithIncludes();

            return _mapper.Map<IEnumerable<LoanResponseDTO>>(loans);
        }
    }
}

