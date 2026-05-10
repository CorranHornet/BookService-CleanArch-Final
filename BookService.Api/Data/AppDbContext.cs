using BookService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookService.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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

            //Prevent deleting users if they have loans
            //User -> Loans
            modelBuilder.Entity<Loan>()
                .HasIndex(l => new { l.MediaUnitId, l.ReturnDate });

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent deleting media units if they have loans
            //MediaUnit -> Loans
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.MediaUnit)
                .WithMany(mu => mu.Loans)
                .HasForeignKey(l => l.MediaUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            //MediaItem -> MediaUnits
            modelBuilder.Entity<MediaUnit>()
                .HasOne(mu => mu.MediaItem)
                .WithMany(m => m.MediaUnits)
                .HasForeignKey(mu => mu.MediaItemId)
                .OnDelete(DeleteBehavior.Restrict);

            //Genre -> MediaItems
            modelBuilder.Entity<MediaItem>()
                .HasOne(m => m.Genre)
                .WithMany(g => g.MediaItems)
                .HasForeignKey(m => m.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}