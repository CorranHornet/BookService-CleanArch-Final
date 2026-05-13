using MediatR;

namespace BookService.Application.MediaItems.Commands
{
    public class UpdateMediaItemCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? GenreId { get; set; }
        public string? Description { get; set; }
        public string? Creator { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int? PageCount { get; set; }
        public int? DurationMinutes { get; set; }
        public int? TrackCount { get; set; }
        public string? Publisher { get; set; }
        public string? Language { get; set; }
        public string? MediaType { get; set; }
    }
}