using BookService.Api.DTOs;

namespace BookService.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllAsync();
        Task<UserResponseDTO?> GetByIdAsync(int id);
        Task<UserResponseDTO> CreateAsync(UserCreateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
