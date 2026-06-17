using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookService.Infrastructure.Repositories
{
    public class MediaItemRepository : IMediaItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MediaItemRepository> _logger;

        public MediaItemRepository(
            ApplicationDbContext context,
            ILogger<MediaItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<MediaItem>> GetAll(string? search)
        {
            _logger.LogInformation("GetAll MediaItems started. Search={Search}", search);

            var query = _context.MediaItems
                .Include(m => m.Genre)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Title != null &&
                    x.Title.ToLower().Contains(search.ToLower()));
            }

            var result = await query.ToListAsync();

            _logger.LogInformation("GetAll MediaItems result count: {Count}", result.Count);

            foreach (var item in result)
            {
                _logger.LogInformation(
                    "MediaItem loaded: Id={Id}, Title={Title}, GenreId={GenreId}, GenreLoaded={GenreLoaded}",
                    item.Id,
                    item.Title,
                    item.GenreId,
                    item.Genre != null
                );
            }

            return result;
        }

        public async Task<MediaItem?> GetById(int id)
        {
            _logger.LogInformation("GetById MediaItem started. Id={Id}", id);

            var result = await _context.MediaItems
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (result == null)
            {
                _logger.LogWarning("MediaItem NOT FOUND. Id={Id}", id);
                return null;
            }

            _logger.LogInformation(
                "MediaItem loaded: Id={Id}, Title={Title}, Genre={Genre}",
                result.Id,
                result.Title,
                result.Genre?.Name
            );

            return result;
        }

        public Task<Genre?> GetGenre(int id)
        {
            _logger.LogInformation("GetGenre called. Id={Id}", id);
            return _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }

        public Task<bool> GenreExists(int id)
            => _context.Genres.AnyAsync(g => g.Id == id);

        public Task<bool> HasMediaUnits(int mediaItemId)
            => _context.MediaUnits.AnyAsync(mu => mu.MediaItemId == mediaItemId);

        public async Task Add(MediaItem item)
        {
            _logger.LogInformation("Adding MediaItem: {@Item}", item);
            await _context.MediaItems.AddAsync(item);
        }

        public Task Delete(MediaItem item)
        {
            _logger.LogWarning("Deleting MediaItem: Id={Id}", item.Id);
            _context.MediaItems.Remove(item);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            _logger.LogInformation("Saving changes to database");
            return _context.SaveChangesAsync();
        }
    }
}