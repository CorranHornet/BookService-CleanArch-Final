namespace BookService.Application.DTOs
{
    public class MediaItemResponseDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int GenreId { get; set; }
        public string Genre { get; set; } = string.Empty; //Only send the Genre name (not the full ojbect)
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