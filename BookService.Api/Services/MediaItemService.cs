using BookService.Infrastructure.Persistence;
using BookService.Api.DTOs;
using BookService.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace BookService.Api.Services
{
    public class MediaItemService : IMediaItemService
    {
        private readonly ApplicationDbContext _context;

        public MediaItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET ALL
        public async Task<IEnumerable<MediaItemResponseDTO>> GetAllAsync(string? search = null)
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

            return await query
                .Select(t => new MediaItemResponseDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,

                    GenreId = t.GenreId,
                    Genre = t.Genre.Name,

                    Creator = t.Creator,
                    ReleaseDate = t.ReleaseDate,
                    ScheduledDate = t.ScheduledDate,
                    PageCount = t.PageCount,
                    DurationMinutes = t.DurationMinutes,
                    TrackCount = t.TrackCount,
                    Publisher = t.Publisher,
                    Language = t.Language,
                    MediaType = t.MediaType
                })
                .ToListAsync();
        }

        // GET BY ID
        public async Task<MediaItemResponseDTO?> GetByIdAsync(int id)
        {
            return await _context.MediaItems
                .Include(m => m.Genre)
                .Where(m => m.Id == id)
                .Select(t => new MediaItemResponseDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,

                    GenreId = t.GenreId,
                    Genre = t.Genre.Name,

                    Creator = t.Creator,
                    ReleaseDate = t.ReleaseDate,
                    ScheduledDate = t.ScheduledDate,
                    PageCount = t.PageCount,
                    DurationMinutes = t.DurationMinutes,
                    TrackCount = t.TrackCount,
                    Publisher = t.Publisher,
                    Language = t.Language,
                    MediaType = t.MediaType
                })
                .FirstOrDefaultAsync();
        }

        // CREATE
        public async Task<MediaItemResponseDTO> CreateAsync(MediaItemCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required");

            // validate genre exists
            var genre = await _context.Genres
                .FirstOrDefaultAsync(g => g.Id == dto.GenreId);

            if (genre == null)
                throw new ArgumentException("Invalid GenreId");

            var mediaItem = new MediaItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Creator = dto.Creator,
                ReleaseDate = dto.ReleaseDate,
                ScheduledDate = dto.ScheduledDate,
                PageCount = dto.PageCount,
                DurationMinutes = dto.DurationMinutes,
                TrackCount = dto.TrackCount,
                Publisher = dto.Publisher,
                Language = dto.Language,
                MediaType = dto.MediaType,

                GenreId = genre.Id
            };

            _context.MediaItems.Add(mediaItem);
            await _context.SaveChangesAsync();

            return new MediaItemResponseDTO
            {
                Id = mediaItem.Id,
                Title = mediaItem.Title,

                GenreId = mediaItem.GenreId,
                Genre = genre.Name,

                Description = mediaItem.Description,
                Creator = mediaItem.Creator,
                ReleaseDate = mediaItem.ReleaseDate,
                ScheduledDate = mediaItem.ScheduledDate,
                PageCount = mediaItem.PageCount,
                DurationMinutes = mediaItem.DurationMinutes,
                TrackCount = mediaItem.TrackCount,
                Publisher = mediaItem.Publisher,
                Language = mediaItem.Language,
                MediaType = mediaItem.MediaType
            };
        }

        // UPDATE
        public async Task<bool> UpdateAsync(int id, MediaItemUpdateDTO dto)
        {
            var mediaItem = await _context.MediaItems
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mediaItem == null)
                return false;

            if (!string.IsNullOrWhiteSpace(dto.Title))
                mediaItem.Title = dto.Title;

            if (dto.ReleaseDate.HasValue)
                mediaItem.ReleaseDate = dto.ReleaseDate.Value;

            if (dto.ScheduledDate.HasValue)
                mediaItem.ScheduledDate = dto.ScheduledDate.Value;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                mediaItem.Description = dto.Description;

            if (!string.IsNullOrWhiteSpace(dto.Creator))
                mediaItem.Creator = dto.Creator;

            if (dto.PageCount.HasValue)
                mediaItem.PageCount = dto.PageCount.Value;

            if (dto.DurationMinutes.HasValue)
                mediaItem.DurationMinutes = dto.DurationMinutes.Value;

            if (dto.TrackCount.HasValue)
                mediaItem.TrackCount = dto.TrackCount.Value;

            if (!string.IsNullOrWhiteSpace(dto.Publisher))
                mediaItem.Publisher = dto.Publisher;

            if (!string.IsNullOrWhiteSpace(dto.Language))
                mediaItem.Language = dto.Language;

            if (!string.IsNullOrWhiteSpace(dto.MediaType))
                mediaItem.MediaType = dto.MediaType;

            if (dto.GenreId.HasValue)
            {
                var genreExists = await _context.Genres
                    .AnyAsync(g => g.Id == dto.GenreId.Value);

                if (!genreExists)
                    return false;

                mediaItem.GenreId = dto.GenreId.Value;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var hasUnits = await _context.MediaUnits
                .AnyAsync(mu => mu.MediaItemId == id);

            if (hasUnits)
                return false;

            var mediaItem = await _context.MediaItems
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mediaItem == null)
                return false;

            _context.MediaItems.Remove(mediaItem);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}