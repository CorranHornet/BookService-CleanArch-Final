using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BookService.Application.Interfaces;

namespace BookService.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Genre>> GetAll()
            => _context.Genres.ToListAsync();

        public Task<Genre?> GetById(int id)
            => _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

        public Task<bool> HasMediaItems(int genreId)
            => _context.MediaItems.AnyAsync(m => m.GenreId == genreId);

        public async Task Add(Genre genre)
            => await _context.Genres.AddAsync(genre);

        public Task Delete(Genre genre)
        {
            _context.Genres.Remove(genre);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}