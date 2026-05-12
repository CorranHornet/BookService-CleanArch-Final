using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.Users.Queries
{
    public record GetUsersQuery : IRequest<List<UserResponseDTO>>;
}