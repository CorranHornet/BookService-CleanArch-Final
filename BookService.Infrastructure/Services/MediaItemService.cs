using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.SignalR.Protocol;

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
        return items.Adapt<IEnumerable<MediaItemResponseDTO>>();
    }

    public async Task<MediaItemResponseDTO?> GetByIdAsync(int id)
    {
        var t = await _repo.GetById(id);
        if (t == null) return null;

        return t.Adapt<MediaItemResponseDTO>();
    }

    public async Task<MediaItemResponseDTO> CreateAsync(MediaItemCreateDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ArgumentException("Title is required");

        var genre = await _repo.GetGenre(dto.GenreId);
        if (genre == null)
            throw new ArgumentException("Invalid GenreId");

        var mediaItem = dto.Adapt<MediaItem>();
        mediaItem.GenreId = genre.Id;

        await _repo.Add(mediaItem);
        await _repo.Save();

        return mediaItem.Adapt<MediaItemResponseDTO>();
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
