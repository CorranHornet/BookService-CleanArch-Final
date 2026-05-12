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
    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreResponseDTO?>
    {
        private readonly IGenreRepository _repo;

        public GetGenreByIdQueryHandler(IGenreRepository repo)
        {
            _repo = repo;
        }

        public async Task<GenreResponseDTO?> Handle(GetGenreByIdQuery request, CancellationToken ct)
        {
            var genre = await _repo.GetById(request.Id);

            if (genre == null) return null;

            return new GenreResponseDTO
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }
    }
}
