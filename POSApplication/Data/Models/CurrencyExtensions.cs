namespace POSApplication.Data.Models
{
    // A static class containing extension methods for currency-related functionality.
    public static class CurrencyExtensions
    {
        /// <summary>
        /// Displays details about the change result to the console.
        /// This is an extension method for the `Change` class.
        /// </summary>
        /// <param name="change">The `Change` object containing denominations to display.</param>
        public static void DisplayChange(this Change change)
        {
            // Iterates over each denomination in the change collection
            foreach (var item in change.Denominations)
            {
                // Prints the denomination value and its count in readable format
                Console.WriteLine($"{item.Value} x {item.Key:C}"); // Example output: "3 x $10"
            }
        }

        /// <summary>
        /// Retrieves the currency code from the ICurrencyConfig implementation.
        /// This is an extension method for the `ICurrencyConfig` interface to simplify access to the currency code.
        /// </summary>
        /// <param name="currencyConfig">The `ICurrencyConfig` object to retrieve the currency code from.</param>
        /// <returns>
        /// The currency code as a string (e.g., "USD").
        /// If no currency is configured, it defaults to "USD".
        /// </returns>
        public static string GetCurrencyCode(this ICurrencyConfig currencyConfig)
        {
            // Tries to get the configured currency's code. Returns "USD" if no currency is set.
            return currencyConfig.GetCurrency()?.CurrencyCode ?? "USD";
        }
    }
}