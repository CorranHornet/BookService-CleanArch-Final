using MediatR;

namespace BookService.Application.Users.Commands
{
    public record DeleteUserCommand(int Id) : IRequest<bool>;
}