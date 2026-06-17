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
}

  
   