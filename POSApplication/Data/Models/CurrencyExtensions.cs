namespace POSApplication.Data.Models
{
    public static class CurrencyExtensions
    {
        // Displays details about the change result
        public static void DisplayChange(this Change change)
        {
            foreach (var item in change.Denominations)
            {
                Console.WriteLine($"{item.Value} x {item.Key:C}");
            }
        }

        // Gets the currency code (helper method)
        public static string GetCurrencyCode(this ICurrencyConfig currencyConfig)
        {
            return currencyConfig.GetCurrency()?.CurrencyCode ?? "USD";
        }
    }
}