using BookService.Application.DTOs;
using BookService.Domain.Entities;
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

            var entity = new MediaItem
            {
                Title = request.Title,
                GenreId = request.GenreId,
                Description = request.Description,
                Creator = request.Creator,
                ReleaseDate = request.ReleaseDate,
                ScheduledDate = request.ScheduledDate,
                PageCount = request.PageCount,
                DurationMinutes = request.DurationMinutes,
                TrackCount = request.TrackCount,
                Publisher = request.Publisher,
                Language = request.Language,
                MediaType = request.MediaType
            };

            await _repo.Add(entity);
            await _repo.Save();

            return new MediaItemResponseDTO
            {
                Id = entity.Id,
                Title = entity.Title,
                GenreId = entity.GenreId,
                Genre = genre.Name,
                Description = entity.Description,
                Creator = entity.Creator,
                ReleaseDate = entity.ReleaseDate,
                ScheduledDate = entity.ScheduledDate,
                PageCount = entity.PageCount,
                DurationMinutes = entity.DurationMinutes,
                TrackCount = entity.TrackCount,
                Publisher = entity.Publisher,
                Language = entity.Language,
                MediaType = entity.MediaType
            };
        }
    }
}