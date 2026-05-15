using BookService.Application.DTOs;
using BookService.Domain.Entities;
using Mapster;
using MediatR;

namespace BookService.Application.MediaItems.Commands
{
    public class CreateMediaItemHandler
        : IRequestHandler<CreateMediaItemCommand, MediaItemResponseDTO>
    {
        private readonly IMediaItemRepository _repo;

        public CreateMediaItemHandler(IMediaItemRepository repo)
        {
            _repo = repo;
        }

        public async Task<MediaItemResponseDTO> Handle(
            CreateMediaItemCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required");

            var genre = await _repo.GetGenre(request.GenreId);

            if (genre == null)
                throw new ArgumentException("Invalid GenreId");

            // 2. Use Mapster to map Command -> Entity automatically
            var entity = request.Adapt<MediaItem>();

            await _repo.Add(entity);
            await _repo.Save();

            // 3. Use Mapster to handle Entity -> DTO. 
            // Because we need the Genre Name string populated, Mapster will look 
            // at the custom rule we add to MapsterConfig.cs to pull it off the 'genre' object.
            return entity.Adapt<MediaItemResponseDTO>();
        }
    }
}