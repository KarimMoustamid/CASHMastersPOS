namespace POSApplication.BusinessLogic.Utilities.logs
{
    using Services;

    public static class Display
    {
        public static void DisplayAvailableCurrencies(Logger logger)
        {
            var availableCurrencies = CurrencyConfig.Instance.GetAvailableCurrencies();
            logger.LogInformation("\nAvailable Currencies:\n");
            foreach (var currency in availableCurrencies)
            {
                Console.WriteLine($"- {currency.Country} ({currency.CurrencyCode})");
            }
        }

        public static T GetInput<T>(Logger logger, string prompt, string errorMessage)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                try
                {
                    return (T) Convert.ChangeType(input, typeof(T));
                }
                catch
                {
                    logger.LogWarning(errorMessage);
                }
            }
        }

        public static Dictionary<decimal, int> CollectPaymentInput(Logger logger)
        {
            var paymentInDenominations = new Dictionary<decimal, int>();
            while (true)
            {
                try
                {
                    Console.Write("Denomination: ");
                    var denom = decimal.Parse(Console.ReadLine() ?? "0");
                    Console.Write("Count: ");
                    var count = int.Parse(Console.ReadLine() ?? "0");

                    if (paymentInDenominations.ContainsKey(denom))
                        paymentInDenominations[denom] += count;
                    else
                        paymentInDenominations[denom] = count;

                    Console.Write("Add another denomination? (y/n): ");
                    if (Console.ReadLine()?.ToLower() != "y")
                        break;
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Invalid input. Error: {ex.Message}");
                }
            }

            return paymentInDenominations;
        }
    }
}