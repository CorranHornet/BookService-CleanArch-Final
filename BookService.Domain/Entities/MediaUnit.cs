using System.ComponentModel.DataAnnotations.Schema;

namespace BookService.Domain.Entities
{

    [Table("MediaUnits")]
    public class MediaUnit
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Number { get; set; }
        public int DurationMinutes { get; set; }

        public int MediaItemId { get; set; }
        public MediaItem? MediaItem { get; set; }

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}