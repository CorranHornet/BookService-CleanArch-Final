using BookService.Application.Interfaces;
using BookService.Application.MediaItems.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.MediaItems.Validators
{
    public class UpdateMediaItemCommandValidator : AbstractValidator<UpdateMediaItemCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateMediaItemCommandValidator(IApplicationDbContext context)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title)
                    .NotEmpty()
                    .MaximumLength(200);
            });

            When(x => x.GenreId.HasValue, () =>
            {
                RuleFor(x => x.GenreId!.Value)
                    .GreaterThan(0)
                    .MustAsync(GenreExists)
                    .WithMessage(x => $"GenreId {x.GenreId} does not exist.");
            });

            _context = context;
        }

        private Task<bool> GenreExists(int genreId, CancellationToken ct)
        {
            return _context.Genres.AnyAsync(g => g.Id == genreId, ct);
        }
    }
}
