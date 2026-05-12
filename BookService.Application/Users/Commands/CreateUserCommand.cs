using BookService.Application.DTOs;
using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.Users.Commands
{
    public record CreateUserCommand(string Username, string Email) : IRequest<UserResponseDTO>;
}