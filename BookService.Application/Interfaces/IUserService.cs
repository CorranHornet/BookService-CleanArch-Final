using BookService.Application.DTOs;


namespace BookService.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllAsync();
        Task<UserResponseDTO?> GetByIdAsync(int id);
        Task<UserResponseDTO> CreateAsync(UserCreateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
