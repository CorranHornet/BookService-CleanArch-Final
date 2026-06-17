using System.ComponentModel.DataAnnotations.Schema;

namespace BookService.Domain.Entities
{
    [Table("MediaItems")]
    public class MediaItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        // FK → Genre
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;

        public string? Creator { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Publisher { get; set; }
        public string? Language { get; set; }
        public DateTime? ScheduledDate { get; set; }

        public ICollection<MediaUnit> MediaUnits { get; set; } = new List<MediaUnit>();
    }
}