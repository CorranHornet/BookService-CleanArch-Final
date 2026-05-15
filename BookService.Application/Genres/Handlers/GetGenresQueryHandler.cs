using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using Mapster;
using MediatR;

namespace BookService.Application.Genres.Queries
{
    internal class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, List<GenreResponseDTO>>
    {
        private readonly IGenreRepository _repo;

        public GetGenresQueryHandler(IGenreRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<GenreResponseDTO>> Handle(GetGenresQuery request, CancellationToken ct)
        {
            var genres = await _repo.GetAll();

            return genres.Adapt<List<GenreResponseDTO>>();
        }
    }
}
