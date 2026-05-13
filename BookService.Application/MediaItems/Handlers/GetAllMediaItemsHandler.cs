using BookService.Application.DTOs;
using BookService.Application.MediaItems.Queries;
using MediatR;

namespace BookService.Application.MediaItems.Handlers
{
    public class GetAllMediaItemsHandler
        : IRequestHandler<GetAllMediaItemsQuery, List<MediaItemResponseDTO>>
    {
        private readonly IMediaItemRepository _repo;

        public GetAllMediaItemsHandler(IMediaItemRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<MediaItemResponseDTO>> Handle(
            GetAllMediaItemsQuery request,
            CancellationToken cancellationToken)
        {
            var items = await _repo.GetAll(request.Search);

            return items.Select(item => new MediaItemResponseDTO
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
            }).ToList();
        }
    }
}