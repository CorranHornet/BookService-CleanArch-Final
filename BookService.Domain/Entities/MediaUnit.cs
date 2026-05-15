namespace BookService.Domain.Entities
{
    // 'abstract' means can't create a "plain" MediaUnit anymore.
    // It MUST be either a Book or an Audiobook.
    public abstract class MediaUnit
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? Number { get; set; }

        // Common Foreign Key
        public int MediaItemId { get; set; }
        public MediaItem MediaItem { get; set; } = null!;

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }

    public class PhysicalBookUnit : MediaUnit
    {
        public int PageCount { get; set; }
    }

    public class AudiobookUnit : MediaUnit
    {
        public int DurationMinutes { get; set; }
    }
}