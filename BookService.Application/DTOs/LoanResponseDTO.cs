using BookService.Domain.Entities;

namespace BookService.Application.DTOs
{
    public class LoanResponseDTO
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public UserDTO? User { get; set; }
        public MediaUnitDTO? MediaUnit { get; set; } 
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class MediaUnitDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Number { get; set; }
        public int MediaItemId { get; set; }
    }
}
