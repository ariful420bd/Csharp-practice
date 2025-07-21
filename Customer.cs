// Models/Customer.cs
namespace ArifTestApi.Models
{
    // Represents the Customer table structure
    public class Customer
    {
        public int CustomerID { get; set; }
        public string? Name { get; set; }
        public string? ContactNo { get; set; }
        public string? Address { get; set; }
        public string? Reference { get; set; }
    }
}