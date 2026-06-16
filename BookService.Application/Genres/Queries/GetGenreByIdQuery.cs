using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.Genres.Queries
{
    public record GetGenreByIdQuery(int Id) : IRequest<GenreResponseDTO?>;
}
