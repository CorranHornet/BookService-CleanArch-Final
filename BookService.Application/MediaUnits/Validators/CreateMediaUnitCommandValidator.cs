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
        }

        private async Task<bool> MediaItemExists(int mediaItemId, CancellationToken ct)
        {
            return await _context.MediaItems.AnyAsync(m => m.Id == mediaItemId, ct);
        }
    }
}
