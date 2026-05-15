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
            return units.Select(MapToResponseDTO);
        }

        public async Task<MediaUnitResponseDTO?> GetByIdAsync(int id)
        {
            var mu = await _repo.GetById(id);
            if (mu == null) return null;

            return MapToResponseDTO(mu);
        }

        public async Task<MediaUnitResponseDTO> CreateAsync(MediaUnitCreateDTO dto)
        {
            if (!await _repo.MediaItemExists(dto.MediaItemId))
                throw new Exception("MediaItem not found");

            MediaUnit entity;

            // Decide which concrete class to create based on the DTO
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
                    // Fixes CS0266: Provide 0 if PageCount is null
                    PageCount = dto.PageCount ?? 0 
                };
            }

            entity.Title = dto.Title ?? string.Empty;
            entity.Number = dto.Number;
            entity.MediaItemId = dto.MediaItemId;

            await _repo.Add(entity);
            await _repo.Save();

            return MapToResponseDTO(entity);
        }

        public async Task<bool> UpdateAsync(int id, MediaUnitUpdateDTO dto)
        {
            var entity = await _repo.GetById(id);
            if (entity == null) return false;

            // Update common fields
            if (!string.IsNullOrWhiteSpace(dto.Title))
                entity.Title = dto.Title;

            entity.Number = dto.Number ?? entity.Number;

            // Fixes CS0266: Pattern match to safe-cast to the correct subtype
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

        /// <summary>
        /// Maps the abstract MediaUnit to a flat DTO for the API response.
        /// </summary>
        private MediaUnitResponseDTO MapToResponseDTO(MediaUnit mu)
        {
            var dto = new MediaUnitResponseDTO
            {
                Id = mu.Id,
                Title = mu.Title,
                Number = mu.Number,
                MediaItemId = mu.MediaItemId
            };

            // Identify type and extract specific data
            if (mu is PhysicalBookUnit book)
            {
                dto.PageCount = book.PageCount;
                dto.UnitType = "Book";
            }
            else if (mu is AudiobookUnit audio)
            {
                dto.DurationMinutes = audio.DurationMinutes;
                dto.UnitType = "Audiobook";
            }

            return dto;
        }
    }
}