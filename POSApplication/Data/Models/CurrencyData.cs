namespace POSApplication.Data.Models
{
    public class CurrencyData
    {
        public string? Country { get; set; }

        public string? CurrencyCode { get; set; }
        public List<decimal>? Denominations { get; set; }
    }
}