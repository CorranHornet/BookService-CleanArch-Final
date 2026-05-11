using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Infrastructure.Repositories;

namespace BookService.Infrastructure.Services
{
    public class MediaUnitService : IMediaUnitService
    {
        private readonly IMediaUnitRepository _repo;

        public MediaUnitService(IMediaUnitRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MediaUnitResponseDTO>> GetAllAsync()
        {
            var units = await _repo.GetAll();

            return units.Select(mu => new MediaUnitResponseDTO
            {
                Id = mu.Id,
                Title = mu.Title,
                Number = mu.Number,
                DurationMinutes = mu.DurationMinutes,
                MediaItemId = mu.MediaItemId
            });
        }

        public async Task<MediaUnitResponseDTO?> GetByIdAsync(int id)
        {
            var mu = await _repo.GetById(id);
            if (mu == null) return null;

            return new MediaUnitResponseDTO
            {
                Id = mu.Id,
                Title = mu.Title,
                Number = mu.Number,
                DurationMinutes = mu.DurationMinutes,
                MediaItemId = mu.MediaItemId
            };
        }

        public async Task<MediaUnitResponseDTO> CreateAsync(MediaUnitCreateDTO dto)
        {
            if (!await _repo.MediaItemExists(dto.MediaItemId))
                throw new Exception("MediaItem not found");

            var entity = new MediaUnit
            {
                Title = dto.Title,
                Number = dto.Number,
                DurationMinutes = dto.DurationMinutes,
                MediaItemId = dto.MediaItemId
            };

            await _repo.Add(entity);
            await _repo.Save();

            return new MediaUnitResponseDTO
            {
                Id = entity.Id,
                Title = entity.Title,
                Number = entity.Number,
                DurationMinutes = entity.DurationMinutes,
                MediaItemId = entity.MediaItemId
            };
        }

        public async Task<bool> UpdateAsync(int id, MediaUnitUpdateDTO dto)
        {
            var entity = await _repo.GetById(id);
            if (entity == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Title))
                entity.Title = dto.Title;

            if (dto.Number.HasValue)
                entity.Number = dto.Number;

            if (dto.DurationMinutes.HasValue)
                entity.DurationMinutes = dto.DurationMinutes.Value;

            await _repo.Save();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetById(id);
            if (entity == null) return false;

            if (await _repo.HasActiveLoan(id))
                return false;

            await _repo.Delete(entity);
            await _repo.Save();

            return true;
        }

        public Task<bool> IsAvailableAsync(int mediaUnitId)
            => _repo.HasActiveLoan(mediaUnitId)
                .ContinueWith(t => !t.Result);
    }
}