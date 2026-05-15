using BookService.Application.DTOs;
using BookService.Application.Loans.Commands;
using BookService.Domain.Entities;
using MediatR;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using System.ComponentModel;
using Mapster;

namespace BookService.Application.Loans.Handlers
{
    internal class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, LoanResponseDTO>
    {
        private readonly ILoanRepository _repo;


        public CreateLoanCommandHandler(
            ILoanRepository repo)
            
        {
            _repo = repo;
            
        }

        public async Task<LoanResponseDTO> Handle(
            CreateLoanCommand request,
            CancellationToken cancellationToken)
        {
            if (!await _repo.UserExists(request.UserId))
                throw new Exception("User does not exist");

            if (!await _repo.MediaUnitExists(request.MediaUnitId))
                throw new Exception("Media Unit does not exist");
            if (await _repo.IsAlreadyLoaned(request.MediaUnitId))
                throw new Exception("Media Unit already loaned");

            var loan = new Loan
            {
                UserId = request.UserId,
                MediaUnitId = request.MediaUnitId,
                LoanDate = DateTime.UtcNow
            };

            await _repo.AddLoan(loan);
            await _repo.SaveChanges();

            return loan.Adapt<LoanResponseDTO>();

        }
    }

  
}
