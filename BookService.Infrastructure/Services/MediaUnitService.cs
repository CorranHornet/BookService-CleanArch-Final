using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Infrastructure.Repositories;
using Mapster;

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
            return units.Adapt<IEnumerable<MediaUnitResponseDTO>>();
        }

        public async Task<MediaUnitResponseDTO?> GetByIdAsync(int id)
        {
            var mu = await _repo.GetById(id);
            if (mu == null) return null;

            return mu.Adapt<MediaUnitResponseDTO>();
        }

        public async Task<MediaUnitResponseDTO> CreateAsync(MediaUnitCreateDTO dto)
        {
            if (!await _repo.MediaItemExists(dto.MediaItemId))
                throw new Exception("MediaItem not found");

            MediaUnit entity;

            if (dto.DurationMinutes.HasValue && dto.DurationMinutes.Value > 0)
            {
                entity = new AudiobookUnit 
                { 
                    DurationMinutes = dto.DurationMinutes.Value 
                };
            }
            else
            {
                entity = new PhysicalBookUnit 
                { 
                    PageCount = dto.PageCount ?? 0 
                };
            }
            
            await _repo.Add(entity);
            await _repo.Save();

            return entity.Adapt<MediaUnitResponseDTO>();
        }

        public async Task<bool> UpdateAsync(int id, MediaUnitUpdateDTO dto)
        {
            var entity = await _repo.GetById(id);
            if (entity == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Title))
                entity.Title = dto.Title;

            entity.Number = dto.Number ?? entity.Number;

            if (entity is PhysicalBookUnit book && dto.PageCount.HasValue)
            {
                book.PageCount = dto.PageCount.Value;
            }
            else if (entity is AudiobookUnit audio && dto.DurationMinutes.HasValue)
            {
                audio.DurationMinutes = dto.DurationMinutes.Value;
            }

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

        public async Task<bool> IsAvailableAsync(int mediaUnitId)
        {
            var hasActiveLoan = await _repo.HasActiveLoan(mediaUnitId);
            return !hasActiveLoan;
        }
    }
}
