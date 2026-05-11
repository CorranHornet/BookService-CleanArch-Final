using BookService.Application.DTOs;

namespace BookService.Application.Interfaces
{
    public interface IGenreService
    {
        Task<List<GenreResponseDTO>> GetAllGenresAsync();
        Task<GenreResponseDTO> GetGenreByIdAsync(int id);
        Task<GenreResponseDTO> CreateGenreAsync(GenreCreateDTO genreDto);
        Task<bool> DeleteGenreAsync(int genreId);
    }
}