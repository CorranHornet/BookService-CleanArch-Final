namespace BookService.Domain.Entities
{
    public class Loan
    {
        public int Id { get; set; }

        // FK → MediaUnit
        public int MediaUnitId { get; set; }
        public MediaUnit MediaUnit { get; set; } = null!;

        // FK → User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime LoanDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
    }
}