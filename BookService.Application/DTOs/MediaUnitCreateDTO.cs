namespace BookService.Application.DTOs
{
    public class MediaUnitCreateDTO
    {
        public string? Title { get; set; }
        public int? Number { get; set; }
        public int MediaItemId { get; set; }
        
        [System.ComponentModel.DefaultValue(null)]
        public int? DurationMinutes { get; set; }
        
        [System.ComponentModel.DefaultValue(null)]
        public int? PageCount { get; set; }
    }
}