using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookService.Infrastructure.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly ApplicationDbContext _context;

    public MediaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MediaItem>> GetAllWithDetailsAsync()
    {
        // Real database call including your relationships
        return await _context.MediaItems
            .Include(m => m.Genre)
            .Include(m => m.MediaUnits)
            .ToListAsync();
    }
}