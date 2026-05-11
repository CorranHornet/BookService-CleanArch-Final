using BookService.Application.DTOs;

namespace BookService.Api.Services
{
    public interface IMediaUnitService
    {
        Task<IEnumerable<MediaUnitResponseDTO>> GetAllAsync();//(int? mediaItemId = null);
        Task<MediaUnitResponseDTO?> GetByIdAsync(int id);
        Task<MediaUnitResponseDTO> CreateAsync(MediaUnitCreateDTO dto);
        Task<bool> UpdateAsync(int id, MediaUnitUpdateDTO dto);
        Task<bool> DeleteAsync(int id);

        Task<bool> IsAvailableAsync(int mediaUnitId);
    }
}