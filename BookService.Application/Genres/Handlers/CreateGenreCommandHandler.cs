using BookService.Application.DTOs;
using BookService.Application.Genres.Commands;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using MapsterMapper; 
using MediatR;

namespace BookService.Application.Genres.Handlers
{
    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, GenreResponseDTO>
    {
        private readonly IGenreRepository _repo;
        private readonly IMapper _mapper; // Inject IMapper

        public CreateGenreCommandHandler(IGenreRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<GenreResponseDTO> Handle(CreateGenreCommand request, CancellationToken ct)
        {
            // 1. Map Command to Entity using IMapper
            var genre = _mapper.Map<Genre>(request);

            // 2. Save
            await _repo.Add(genre);
            await _repo.SaveChangesAsync();

            // 3. Map Entity to DTO using IMapper
            return _mapper.Map<GenreResponseDTO>(genre);
        }
    }
}