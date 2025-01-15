namespace POSApplication.Data.Models
{
    public class Payment
    {
        // These models will encapsulate payment information, making it easier for other engineers to understand and extend the functionality
        public decimal TotalPaid { get; set; }
        public Dictionary<decimal, int> Denominations { get; set; } = new();
    }
}