using System.ComponentModel.DataAnnotations.Schema;

namespace BookService.Domain.Entities
{
    [Table("MediaUnits")]
    public class MediaUnit
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int Number { get; set; }

        public int DurationMinutes { get; set; }

        // FK → MediaItem
        public int MediaItemId { get; set; }
        public MediaItem MediaItem { get; set; } = null!;

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}