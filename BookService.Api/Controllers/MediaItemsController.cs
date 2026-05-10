using BookService.Api.Data;
using BookService.Api.DTOs;
using BookService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MediaItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/mediaitems
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.MediaItems
                .Include(m => m.Genre)
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/mediaitems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.MediaItems
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/mediaitems
        [HttpPost]
        public async Task<IActionResult> Create(MediaItemCreateDTO dto)
        {
            var genreExists = await _context.Genres
                .AnyAsync(g => g.Id == dto.GenreId);

            if (!genreExists)
                return BadRequest("Invalid GenreId");

            var mediaItem = new MediaItem
            {
                Title = dto.Title,
                GenreId = dto.GenreId,
                Description = dto.Description,
                Creator = dto.Creator,
                ReleaseDate = dto.ReleaseDate,
                ScheduledDate = dto.ScheduledDate,
                PageCount = dto.PageCount,
                DurationMinutes = dto.DurationMinutes,
                TrackCount = dto.TrackCount,
                Publisher = dto.Publisher,
                Language = dto.Language,
                MediaType = dto.MediaType
            };

            _context.MediaItems.Add(mediaItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = mediaItem.Id }, mediaItem);
        }

        // PUT: api/mediaitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MediaItemCreateDTO dto)
        {
            var item = await _context.MediaItems.FindAsync(id);

            if (item == null)
                return NotFound();

            var genreExists = await _context.Genres
                .AnyAsync(g => g.Id == dto.GenreId);

            if (!genreExists)
                return BadRequest("Invalid GenreId");

            item.Title = dto.Title;
            item.GenreId = dto.GenreId;
            item.Description = dto.Description;
            item.Creator = dto.Creator;
            item.ReleaseDate = dto.ReleaseDate;
            item.ScheduledDate = dto.ScheduledDate;
            item.PageCount = dto.PageCount;
            item.DurationMinutes = dto.DurationMinutes;
            item.TrackCount = dto.TrackCount;
            item.Publisher = dto.Publisher;
            item.Language = dto.Language;
            item.MediaType = dto.MediaType;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/mediaitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.MediaItems.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.MediaItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}