using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.MediaItems.Queries
{
    public class GetAllMediaItemsQuery : IRequest<List<MediaItemResponseDTO>>
    {
        public string? Search { get; set; }

        public GetAllMediaItemsQuery(string? search = null)
        {
            Search = search;
        }
    }
}