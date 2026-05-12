using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return genres.Select(g => new GenreResponseDTO
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
        }
    }
}
