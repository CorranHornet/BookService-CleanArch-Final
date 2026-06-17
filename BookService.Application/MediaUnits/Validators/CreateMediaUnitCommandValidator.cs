using BookService.Application.Interfaces;
using BookService.Application.MediaUnits.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.MediaUnits.Validators
{
    public class CreateMediaUnitCommandValidator : AbstractValidator<CreateMediaUnitCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateMediaUnitCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3)
                .WithMessage("Title must be at least 3 characters long.");

            RuleFor(x => x.MediaItemId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                .WithMessage("MediaItemId must be greater than 0.")
                .MustAsync(MediaItemExists)
                .WithMessage("The specified MediaItem does not exist.");

            RuleFor(x => x)
                .Must(x =>
                    (x.PageCount.HasValue && !x.DurationMinutes.HasValue) ||
                    (!x.PageCount.HasValue && x.DurationMinutes.HasValue))
                .WithMessage("Specify either PageCount or DurationMinutes, not both, and one is required.");

            RuleFor(x => x.PageCount)
                .GreaterThan(0)
                .When(x => x.PageCount.HasValue);

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0)
                .When(x => x.DurationMinutes.HasValue);
        }

        private async Task<bool> MediaItemExists(int mediaItemId, CancellationToken ct)
        {
            return await _context.MediaItems.AnyAsync(m => m.Id == mediaItemId, ct);
        }
    }
}
