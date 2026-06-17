namespace BookService.Application.DTOs
{
    public class MediaUnitDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Number { get; set; }
        public int MediaItemId { get; set; }

        public int? PageCount { get; set; }
        public int? DurationMinutes { get; set; }

        public string UnitType { get; set; } = string.Empty;
    }
}