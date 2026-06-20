namespace BookService.Application.DTOs
{
    public class GenreResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }  = string.Empty;

        // Flatten the MediaItems relationship, but only return necessary fields
        public List<MediaItemResponseDTO> MediaItems { get; set; } = new List<MediaItemResponseDTO>(); //Only include relevant fields for MediaItems
    }
}