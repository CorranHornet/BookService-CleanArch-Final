using BookService.Api.DTOs;
using BookService.Api.Services;
using Microsoft.AspNetCore.Mvc;


namespace BookService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: api/genre
        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _genreService.GetAllGenresAsync();
            return Ok(genres);
        }

        // POST: api/genre
        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreCreateDTO genreDto)
        {
            var genre = await _genreService.CreateGenreAsync(genreDto);
            return CreatedAtAction(nameof(GetGenres), new { id = genre.Id }, genre);
        }

        // DELETE: api/genre/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var result = await _genreService.DeleteGenreAsync(id);

            if (!result)
            {
                return NotFound("Genre not found or has associated MediaItems.");
            }

            return Ok("Genre deleted successfully.");
        }
    }
}