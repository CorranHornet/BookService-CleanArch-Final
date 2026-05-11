using BookService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BookService.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<MediaItem> MediaItems { get; }
        DbSet<MediaUnit> MediaUnits { get; }
        DbSet<Genre> Genres { get; }
        DbSet<Loan> Loans { get; }
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}