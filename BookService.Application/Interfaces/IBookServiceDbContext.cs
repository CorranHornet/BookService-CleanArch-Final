using BookService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Common.Interfaces
{
    public interface IBookServiceDbContext
    {
        // The Application layer only cares that a list of MediaItems exists
        DbSet<MediaItem> MediaItems { get; }
        DbSet<MediaUnit> MediaUnits { get; }
        DbSet<Genre> Genres { get; }
        DbSet<Loan> Loans { get; }
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}


