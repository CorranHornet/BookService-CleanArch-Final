using BookService.Domain.Entities;

namespace BookService.Application.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAll();
        Task<Genre?> GetById(int id);

        Task Add(Genre genre);
        Task Delete(Genre genre);

        Task<bool> HasMediaItems(int genreId);
        Task SaveChangesAsync();
    }
}