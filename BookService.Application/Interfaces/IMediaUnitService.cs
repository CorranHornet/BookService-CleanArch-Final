using BookService.Application.DTOs;

namespace BookService.Application.Interfaces
{
    public interface IMediaUnitService
    {
        Task<IEnumerable<MediaUnitDTO>> GetAllAsync();
        Task<MediaUnitDTO?> GetByIdAsync(int id);
        Task<MediaUnitDTO> CreateAsync(MediaUnitCreateDTO dto);
        Task<bool> UpdateAsync(int id, MediaUnitUpdateDTO dto);
        Task<bool> DeleteAsync(int id);

        Task<bool> IsAvailableAsync(int mediaUnitId);
    }
}