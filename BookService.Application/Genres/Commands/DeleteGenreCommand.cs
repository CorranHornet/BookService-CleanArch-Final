using MediatR;

namespace BookService.Application.Genres.Commands
{
    public record DeleteGenreCommand(int Id) : IRequest<bool>;
}
