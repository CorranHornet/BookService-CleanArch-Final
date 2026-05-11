using BookService.Infrastructure.Persistence;
using BookService.Application.Interfaces;
using BookService.Infrastructure.Services;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Application.DTOs;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookService.Infrastructure.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GenreResponseDTO>> GetAllGenresAsync()
        {
            var genres = await _context.Genres
                .Select(g => new GenreResponseDTO
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();

            return genres;
        }

        public async Task<GenreResponseDTO> GetGenreByIdAsync(int id)
        {
            var genre = await _context.Genres
                .Where(g => g.Id == id)
                .Select(g => new GenreResponseDTO
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .FirstOrDefaultAsync();

            return genre;
        }

        public async Task<GenreResponseDTO> CreateGenreAsync(GenreCreateDTO genreDto)
        {
            var genre = new Genre
            {
                Name = genreDto.Name
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return new GenreResponseDTO
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public async Task<bool> DeleteGenreAsync(int genreId)
        {
            var genre = await _context.Genres
                .FirstOrDefaultAsync(g => g.Id == genreId);

            if (genre == null)
            {
                return false;  // Genre doesn't exist
            }

            // Check if any media items are linked to the genre
            var mediaItemsLinkedToGenre = await _context.MediaItems
                .AnyAsync(m => m.GenreId == genreId);

            if (mediaItemsLinkedToGenre)
            {
                return false;  // Genre has MediaItems, can't delete it
            }

            // Delete the genre if no MediaItems are linked
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return true;  // Genre deleted successfully
        }
    }
}