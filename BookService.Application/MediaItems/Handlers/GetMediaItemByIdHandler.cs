using BookService.Application.DTOs;
using BookService.Application.MediaItems.Queries;
using Mapster;
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

            return item.Adapt<MediaItemResponseDTO>();
        }
    }
}