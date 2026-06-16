using BookService.Domain.Entities;
using BookService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BookService.Application.Interfaces;

namespace BookService.Infrastructure.Repositories
{
    public class MediaUnitRepository : IMediaUnitRepository
    {
        private readonly ApplicationDbContext _context;

        public MediaUnitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<MediaUnit>> GetAll()
            => _context.MediaUnits.ToListAsync();

        public Task<MediaUnit?> GetById(int id)
            => _context.MediaUnits.FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> MediaItemExists(int mediaItemId)
            => _context.MediaItems.AnyAsync(x => x.Id == mediaItemId);

        public Task<bool> HasActiveLoan(int mediaUnitId)
            => _context.Loans.AnyAsync(l => l.MediaUnitId == mediaUnitId && l.ReturnDate == null);

        public async Task Add(MediaUnit entity)
            => await _context.MediaUnits.AddAsync(entity);

        public Task Delete(MediaUnit entity)
        {
            _context.MediaUnits.Remove(entity);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}