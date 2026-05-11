using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
public class MediaItemRepository : IMediaItemRepository
{
    private readonly ApplicationDbContext _context;

    public MediaItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MediaItem>> GetAll(string? search)
    {
        var query = _context.MediaItems
            .Include(m => m.Genre)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Title != null &&
                x.Title.ToLower().Contains(search.ToLower()));
        }

        return await query.ToListAsync();
    }

    public Task<MediaItem?> GetById(int id)
        => _context.MediaItems
            .Include(m => m.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);

    public Task<Genre?> GetGenre(int id)
        => _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

    public Task<bool> GenreExists(int id)
        => _context.Genres.AnyAsync(g => g.Id == id);

    public Task<bool> HasMediaUnits(int mediaItemId)
        => _context.MediaUnits.AnyAsync(mu => mu.MediaItemId == mediaItemId);

    public async Task Add(MediaItem item)
        => await _context.MediaItems.AddAsync(item);

    public Task Delete(MediaItem item)
    {
        _context.MediaItems.Remove(item);
        return Task.CompletedTask;
    }

    public Task Save()
        => _context.SaveChangesAsync();
}