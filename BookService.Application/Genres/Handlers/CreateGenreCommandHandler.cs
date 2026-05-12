using BookService.Application.DTOs;
using BookService.Application.Genres.Commands;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Application.Genres.Handlers
{
    internal class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, GenreResponseDTO>
    {
        private readonly IGenreRepository _repo;

        public CreateGenreCommandHandler(IGenreRepository repo)
        {
            _repo = repo;
        }

        public async Task<GenreResponseDTO> Handle(CreateGenreCommand request, CancellationToken ct)
        {
            var genre = new Genre
            {
                Name = request.Name
            };

            await _repo.Add(genre);
            await _repo.Save();

            return new GenreResponseDTO
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }
    }
}
