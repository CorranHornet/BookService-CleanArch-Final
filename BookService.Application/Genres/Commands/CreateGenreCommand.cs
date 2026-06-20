using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.Genres.Commands
{
    public record CreateGenreCommand(string Name) : IRequest<GenreResponseDTO>;
}
