using BookService.Application.DTOs;
using BookService.Application.Genres.Commands;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using Mapster;
using MediatR;

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
            // Map Command -> Entity
            var genre = request.Adapt<Genre>();

            await _repo.Add(genre);
            await _repo.Save();

            return genre.Adapt<GenreResponseDTO>();
            
        }
    }
}
