using BookService.Application.Interfaces;
using BookService.Application.MediaItems.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class CreateMediaItemCommandValidator : AbstractValidator<CreateMediaItemCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateMediaItemCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
            .MaximumLength(200);

        RuleFor(x => x.Creator)
            .NotEmpty().WithMessage("Creator is required.");

        RuleFor(x => x.GenreId)
            .GreaterThan(0).WithMessage("GenreId must be a valid positive number.")
            .MustAsync(GenreExists).WithMessage("The selected Genre does not exist.");
    }

    private async Task<bool> GenreExists(int genreId, CancellationToken ct)
    {
        return await _context.Genres.AnyAsync(g => g.Id == genreId, ct);
    }
}