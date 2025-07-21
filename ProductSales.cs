// Models/ProductSales.cs
namespace ArifTestApi.Models
{
    // Represents the ProductSales table structure
    public class ProductSales
    {
        public int SalesID { get; set; }
        public int ProductID { get; set; } // Foreign key to Product
        public string? Category { get; set; }
        public string? ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string? Unit { get; set; }
        public string? Remarks { get; set; }
    }
}