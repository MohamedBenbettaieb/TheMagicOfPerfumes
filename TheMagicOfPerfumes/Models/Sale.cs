namespace TheMagicOfPerfumes.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }

        // Foreign key (nullable — a sale can be anonymous)
        public int? CustomerId { get; set; }

        // Navigation properties
        public Customer? Customer { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}