using BookService.Domain.Entities;

public interface IMediaItemRepository
{
    Task<List<MediaItem>> GetAll(string? search);
    Task<MediaItem?> GetById(int id);

    Task<Genre?> GetGenre(int id);
    Task<bool> GenreExists(int id);

    Task<bool> HasMediaUnits(int mediaItemId);

    Task Add(MediaItem item);
    Task Delete(MediaItem item);

    Task Save();
}