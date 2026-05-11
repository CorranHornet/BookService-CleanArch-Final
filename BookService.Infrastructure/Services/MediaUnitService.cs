using BookService.Infrastructure.Persistence;
using BookService.Application.DTOs;
using BookService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BookService.Application.Interfaces;

namespace BookService.Infrastructure.Services
{
    public class MediaUnitService : IMediaUnitService
    {
        private readonly ApplicationDbContext _context;

        public MediaUnitService(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET ALL
        public async Task<IEnumerable<MediaUnitResponseDTO>> GetAllAsync()
        {
            return await _context.MediaUnits
                .Select(mu => new MediaUnitResponseDTO
                {
                    Id = mu.Id,
                    Title = mu.Title,
                    Number = mu.Number,
                    DurationMinutes = mu.DurationMinutes,
                    MediaItemId = mu.MediaItemId
                })
                .ToListAsync();
        }

        // GET BY ID
        public async Task<MediaUnitResponseDTO?> GetByIdAsync(int id)
        {
            return await _context.MediaUnits
                .Where(mu => mu.Id == id)
                .Select(mu => new MediaUnitResponseDTO
                {
                    Id = mu.Id,
                    Title = mu.Title,
                    Number = mu.Number,
                    DurationMinutes = mu.DurationMinutes,
                    MediaItemId = mu.MediaItemId
                })
                .FirstOrDefaultAsync();
        }

        // CREATE
        public async Task<MediaUnitResponseDTO> CreateAsync(MediaUnitCreateDTO dto)
        {
            var mediaItemExists = await _context.MediaItems
                .AnyAsync(m => m.Id == dto.MediaItemId);

            if (!mediaItemExists)
                throw new Exception("MediaItem not found");

            var entity = new MediaUnit
            {
                Title = dto.Title,
                Number = dto.Number,
                DurationMinutes = dto.DurationMinutes,
                MediaItemId = dto.MediaItemId
            };

            _context.MediaUnits.Add(entity);
            await _context.SaveChangesAsync();

            return new MediaUnitResponseDTO
            {
                Id = entity.Id,
                Title = entity.Title,
                Number = entity.Number,
                DurationMinutes = entity.DurationMinutes,
                MediaItemId = entity.MediaItemId
            };
        }

        // UPDATE
        public async Task<bool> UpdateAsync(int id, MediaUnitUpdateDTO dto)
        {
            var entity = await _context.MediaUnits.FindAsync(id);

            if (entity == null)
                return false;

            if (!string.IsNullOrWhiteSpace(dto.Title))
                entity.Title = dto.Title;

            if (dto.Number.HasValue)
                entity.Number = dto.Number;

            if (dto.DurationMinutes.HasValue)
                entity.DurationMinutes = dto.DurationMinutes.Value;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE (only if NOT borrowed)
        public async Task<bool> DeleteAsync(int id)
        {
            var hasActiveLoan = await _context.Loans
                .AnyAsync(l => l.MediaUnitId == id && l.ReturnDate == null);

            if (hasActiveLoan)
                return false;

            var entity = await _context.MediaUnits.FindAsync(id);

            if (entity == null)
                return false;

            _context.MediaUnits.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // DERIVED AVAILABILITY (THIS IS THE CORRECT DESIGN)
        public async Task<bool> IsAvailableAsync(int mediaUnitId)
        {
            return !await _context.Loans
                .AnyAsync(l => l.MediaUnitId == mediaUnitId && l.ReturnDate == null);
        }
    }
}