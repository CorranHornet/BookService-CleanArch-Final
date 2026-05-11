namespace BookService.Application.DTOs
{
    public class MediaUnitCreateDTO
    {
        public string? Title { get; set; }
        public int? Number { get; set; }
        public int DurationMinutes { get; set; }
        public int MediaItemId { get; set; }
    }
}