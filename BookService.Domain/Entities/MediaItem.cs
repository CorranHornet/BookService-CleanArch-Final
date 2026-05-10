using System.ComponentModel.DataAnnotations.Schema;

namespace BookService.Domain.Entities
{

    [Table("MediaItems")] // maps to existing table
    public class MediaItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        // Genre relationship
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null;

        public string? Creator { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? PageCount { get; set; }
        public int? DurationMinutes { get; set; }
        public int? TrackCount { get; set; }
        public string? Publisher { get; set; }
        public string? Language { get; set; }
        public string? MediaType { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public ICollection<MediaUnit> MediaUnits { get; set; } = new List<MediaUnit>();
    }
}
