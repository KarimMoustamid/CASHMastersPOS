namespace POSApplication.Data.Models
{
    // Represents the structure of a currency configuration file.
    // This model is used to deserialize JSON or other structured data files
    // containing information about multiple currencies.
    public class CurrencyFile
    {
        // A list containing the details of available currencies.
        // Each entry in the list is of type `CurrencyData`, which stores information
        // such as currency code, country, and denominations.
        public List<CurrencyData>? Currencies { get; set; }
    }
}