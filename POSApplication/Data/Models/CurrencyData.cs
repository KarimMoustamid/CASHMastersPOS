namespace POSApplication.Data.Models
{
    // Represents key information about a specific currency.
    // This model is used to encapsulate details such as the country, unique currency code,
    // and the list of valid denominations for the currency.
    public class CurrencyData
    {
        // The name of the country associated with the currency (e.g., "United States", "Canada").
        public string? Country { get; set; }

        // The unique code of the currency (e.g., "USD" for US Dollar, "CAD" for Canadian Dollar).
        // This code is typically used to identify the currency in applications or systems.
        public string? CurrencyCode { get; set; }

        // A list of valid denominations for the currency.
        // Each denomination is stored as a decimal value (e.g., 100 for $100 bill, 0.25 for a quarter coin).
        public List<decimal>? Denominations { get; set; }
    }
}