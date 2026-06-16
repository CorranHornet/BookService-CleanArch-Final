using BookService.Application.Genres.Commands;
using FluentValidation;

namespace BookService.Application.Genres.Validators
{
    public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Genre name is required.")
                .MinimumLength(3).WithMessage("Genre name must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Genre name cannot exceed 50 characters.");
        }
    }
}