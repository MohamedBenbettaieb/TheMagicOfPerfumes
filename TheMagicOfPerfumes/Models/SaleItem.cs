namespace TheMagicOfPerfumes.Models
{
    public class SaleItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }  // price at time of sale

        // Computed — not stored in DB
        public decimal Subtotal => Quantity * UnitPrice;

        // Foreign keys
        public int SaleId { get; set; }
        public int ProductId { get; set; }

        // Navigation properties
        public Sale Sale { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}