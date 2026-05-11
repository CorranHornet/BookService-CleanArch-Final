using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookService.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<MediaItem> MediaItems => Set<MediaItem>();
        public DbSet<MediaUnit> MediaUnits => Set<MediaUnit>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Loan> Loans => Set<Loan>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User -> Loans (Restrict Delete)
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // MediaUnit -> Loans (Restrict Delete)
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.MediaUnit)
                .WithMany(mu => mu.Loans)
                .HasForeignKey(l => l.MediaUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            // MediaItem -> MediaUnits (Restrict Delete)
            modelBuilder.Entity<MediaUnit>()
                .HasOne(mu => mu.MediaItem)
                .WithMany(m => m.MediaUnits)
                .HasForeignKey(mu => mu.MediaItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Genre -> MediaItems (Restrict Delete)
            modelBuilder.Entity<MediaItem>()
                .HasOne(m => m.Genre)
                .WithMany(g => g.MediaItems)
                .HasForeignKey(m => m.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for faster lookups
            modelBuilder.Entity<Loan>()
                .HasIndex(l => new { l.MediaUnitId, l.ReturnDate });
        }
    }
}