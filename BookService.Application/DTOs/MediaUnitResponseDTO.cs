namespace BookService.Application.DTOs
{
    public class MediaUnitResponseDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Number { get; set; }
        public int MediaItemId { get; set; }

        // Make both nullable so the API only shows what is relevant
        public int? PageCount { get; set; }
        public int? DurationMinutes { get; set; }

        // Adding this helps the Frontend know which icon to show (Book vs Headphones)
        public string UnitType { get; set; } = string.Empty; 
    }
}