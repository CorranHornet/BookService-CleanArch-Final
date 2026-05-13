using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.MediaItems.Queries
{
    public class GetMediaItemByIdQuery : IRequest<MediaItemResponseDTO?>
    {
        public int Id { get; set; }

        public GetMediaItemByIdQuery(int id)
        {
            Id = id;
        }
    }
}