using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.Users.Commands
{
    public class CreateUserCommand : IRequest<UserResponseDTO>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}