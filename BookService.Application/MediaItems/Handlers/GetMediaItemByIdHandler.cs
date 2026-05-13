using BookService.Application.DTOs;
using BookService.Application.MediaItems.Queries;
using MediatR;

namespace BookService.Application.MediaItems.Handlers
{
    public class GetMediaItemByIdHandler
        : IRequestHandler<GetMediaItemByIdQuery, MediaItemResponseDTO?>
    {
        private readonly IMediaItemRepository _repo;

        public GetMediaItemByIdHandler(IMediaItemRepository repo)
        {
            _repo = repo;
        }

        public async Task<MediaItemResponseDTO?> Handle(
            GetMediaItemByIdQuery request,
            CancellationToken cancellationToken)
        {
            var item = await _repo.GetById(request.Id);

            if (item == null)
                return null;

            return new MediaItemResponseDTO
            {
                Id = item.Id,
                Title = item.Title,
                GenreId = item.GenreId,
                Genre = item.Genre.Name,
                Description = item.Description,
                Creator = item.Creator,
                ReleaseDate = item.ReleaseDate,
                ScheduledDate = item.ScheduledDate,
                PageCount = item.PageCount,
                DurationMinutes = item.DurationMinutes,
                TrackCount = item.TrackCount,
                Publisher = item.Publisher,
                Language = item.Language,
                MediaType = item.MediaType
            };
        }
    }
}