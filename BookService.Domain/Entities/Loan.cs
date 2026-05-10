namespace BookService.Domain.Entities
{
    public class Loan
    {
        public int Id { get; set; }

        public int MediaUnitId { get; set; }
        public MediaUnit MediaUnit { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime? ReturnDate { get; set; } // null if not returned yet
    }
}