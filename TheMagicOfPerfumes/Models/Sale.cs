using System.ComponentModel.DataAnnotations.Schema;

namespace TheMagicOfPerfumes.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public decimal TotalAmount => SaleItems?.Sum(item => item.Subtotal) ?? 0m;

        // Foreign key (nullable — a sale can be anonymous)
        public int? CustomerId { get; set; }

        // Navigation properties
        public Customer? Customer { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}