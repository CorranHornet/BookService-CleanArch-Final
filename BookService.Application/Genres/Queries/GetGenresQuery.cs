using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.Genres.Queries
{
    public record GetGenresQuery : IRequest<List<GenreResponseDTO>>;
}
