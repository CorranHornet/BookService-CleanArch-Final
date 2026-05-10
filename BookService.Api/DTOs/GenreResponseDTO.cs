

namespace BookService.Api.DTOs
{
    public class GenreResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Flatten the MediaItems relationship, but only return necessary fields
        public List<MediaItemResponseDTO> MediaItems { get; set; } = new List<MediaItemResponseDTO>(); //Only include relevant fields for MediaItems
    }
}