using BookService.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Application.Loans.Queries
{
    public record GetLoansQuery : IRequest<IEnumerable<LoanResponseDTO>>
    {
    }
}
