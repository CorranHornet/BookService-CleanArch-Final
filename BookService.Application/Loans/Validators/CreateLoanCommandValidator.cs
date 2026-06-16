using BookService.Application.Interfaces;
using BookService.Application.Loans.Commands;
using FluentValidation;

namespace BookService.Application.Loans.Validators
{
    public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
    {
        private readonly ILoanRepository _repo;

        public CreateLoanCommandValidator(ILoanRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.UserId)
                .MustAsync(async (id, ct) => await _repo.UserExists(id))
                .WithMessage("User does not exist.");

            RuleFor(x => x.MediaUnitId)
                .MustAsync(async (id, ct) => await _repo.MediaUnitExists(id))
                .WithMessage("MediaUnit does not exist.")
                .DependentRules(() => {
                    RuleFor(x => x.MediaUnitId)
                        .MustAsync(async (id, ct) => !await _repo.IsAlreadyLoaned(id))
                        .WithMessage("MediaUnit is already loaned.");
                });
        }
    }
}
