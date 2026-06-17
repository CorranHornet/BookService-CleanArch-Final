using BookService.Application.DTOs;
using MediatR;

namespace BookService.Application.MediaItems.Commands
{
    public class CreateMediaItemCommand : IRequest<MediaItemResponseDTO>
    {
        public string? Title { get; set; }
        public int GenreId { get; set; }
        public string? Description { get; set; }
        public string? Creator { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string? Publisher { get; set; }
        public string? Language { get; set; }
    }
}