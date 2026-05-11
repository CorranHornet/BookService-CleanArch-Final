using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;

public class MediaItemService : IMediaItemService
{
    private readonly IMediaItemRepository _repo;

    public MediaItemService(IMediaItemRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<MediaItemResponseDTO>> GetAllAsync(string? search = null)
    {
        var items = await _repo.GetAll(search);

        return items.Select(t => new MediaItemResponseDTO
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
        });
    }

    public async Task<MediaItemResponseDTO?> GetByIdAsync(int id)
    {
        var t = await _repo.GetById(id);
        if (t == null) return null;

        return new MediaItemResponseDTO
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
        };
    }

    public async Task<MediaItemResponseDTO> CreateAsync(MediaItemCreateDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Title is required");

        var genre = await _repo.GetGenre(dto.GenreId);
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

        await _repo.Add(mediaItem);
        await _repo.Save();

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

    public async Task<bool> UpdateAsync(int id, MediaItemUpdateDTO dto)
    {
        var item = await _repo.GetById(id);
        if (item == null) return false;

        if (!string.IsNullOrWhiteSpace(dto.Title))
            item.Title = dto.Title;

        if (dto.GenreId.HasValue)
        {
            var exists = await _repo.GenreExists(dto.GenreId.Value);
            if (!exists) return false;

            item.GenreId = dto.GenreId.Value;
        }

        await _repo.Save();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _repo.GetById(id);
        if (item == null) return false;

        if (await _repo.HasMediaUnits(id))
            return false;

        await _repo.Delete(item);
        await _repo.Save();

        return true;
    }
}