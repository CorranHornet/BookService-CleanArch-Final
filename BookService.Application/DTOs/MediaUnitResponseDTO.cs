namespace BookService.Application.DTOs
{
    public class MediaUnitResponseDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Number { get; set; }
        public int DurationMinutes { get; set; }
        public int MediaItemId { get; set; }
    }
}