namespace POSApplication.Data.Models
{
    public class Change
    {
        // These models will encapsulate change information, making it easier for other engineers to understand and extend the functionality
        public Dictionary<decimal, int> Denominations { get; set; } = new();
        public decimal TotalChange { get; set; }
    }
}