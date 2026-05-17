using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Infrastructure.Repositories;
using Mapster;

namespace BookService.Infrastructure.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repo;

        public GenreService(IGenreRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<GenreResponseDTO>> GetAllGenresAsync()
        {
            var genres = await _repo.GetAll();
            return genres.Adapt<List<GenreResponseDTO>>();
        }

        public async Task<GenreResponseDTO?> GetGenreByIdAsync(int id)
        {
            var genre = await _repo.GetById(id);
            if (genre == null) return null;

            return genre.Adapt<GenreResponseDTO>();
        }

        public async Task<GenreResponseDTO> CreateGenreAsync(GenreCreateDTO dto)
        {
            var genre = dto.Adapt<Genre>();
            
            await _repo.Add(genre);
            await _repo.Save();

            return genre.Adapt<GenreResponseDTO>();
        }

        public async Task<bool> DeleteGenreAsync(int genreId)
        {
            var genre = await _repo.GetById(genreId);
            if (genre == null) return false;

            if (await _repo.HasMediaItems(genreId))
                return false;

            await _repo.Delete(genre);
            await _repo.Save();

            return true;
        }
    }
}
