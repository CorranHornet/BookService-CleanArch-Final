using BookService.Application.DTOs;
using BookService.Application.MediaItems.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.SignalR.Protocol;

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



            return items.Adapt<List< MediaItemResponseDTO >> ();
        }
    }
}