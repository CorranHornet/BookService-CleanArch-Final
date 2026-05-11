using BookService.Domain.Entities;

namespace BookService.Application.Interfaces;

public interface IMediaRepository
{
    // Industry standard: Use Task for async and IEnumerable for collections
    Task<IEnumerable<MediaItem>> GetAllWithDetailsAsync();
}