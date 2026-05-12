using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.Users.Queries
{
    public record GetUserByIdQuery(int Id) : IRequest<UserResponseDTO?>;
}