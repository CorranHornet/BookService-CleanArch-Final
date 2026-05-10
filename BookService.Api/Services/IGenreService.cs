using BookService.Api.DTOs;

namespace BookService.Api.Services
{
    public interface IGenreService
    {
        Task<List<GenreResponseDTO>> GetAllGenresAsync();
        Task<GenreResponseDTO> GetGenreByIdAsync(int id);
        Task<GenreResponseDTO> CreateGenreAsync(GenreCreateDTO genreDto);
        Task<bool> DeleteGenreAsync(int genreId);
    }
}