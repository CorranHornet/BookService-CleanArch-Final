using BookService.Domain.Entities;
namespace BookService.Application.Interfaces
{
    public interface IMediaUnitRepository
    {
        Task<List<MediaUnit>> GetAll();
        Task<MediaUnit?> GetById(int id);

        Task<bool> MediaItemExists(int mediaItemId);
        Task<bool> HasActiveLoan(int mediaUnitId);

        Task Add(MediaUnit entity);
        Task Delete(MediaUnit entity);

        Task Save();
    }
}