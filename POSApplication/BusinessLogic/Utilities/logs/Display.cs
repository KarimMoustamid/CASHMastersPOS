    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using POSApplication.BusinessLogic.Services;
    using POSApplication.Data.Models;

    public static class Display
    {
        // Static field for logger
        private readonly static ILogger _logger;

        // Static constructor to initialize the logger
        static Display()
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole(options =>
                {
                    options.FormatterName = "simple"; // Use the custom formatter
                });

                builder.AddConsoleFormatter<SimpleConsoleFormatter, ConsoleFormatterOptions>();
            });

            _logger = loggerFactory.CreateLogger("Display"); // Generic logger for a static class
        }

        public static void DisplayAvailableCurrencies()
        {
            var availableCurrencies = CurrencyConfig.Instance.GetAvailableCurrencies();
            _logger.LogInformation("\nAvailable Currencies:\n");
            foreach (CurrencyData? currency in availableCurrencies) Console.WriteLine($"- {currency.Country} ({currency.CurrencyCode})");
        }

        public static T GetInput<T>(string prompt, string errorMessage)
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
                    _logger.LogWarning(errorMessage);
                }
            }
        }

        public static Dictionary<decimal, int> CollectPaymentInput()
        {
            var paymentInDenominations = new Dictionary<decimal, int>();
            while (true)
                try
                {
                    Console.Write("Denomination: ");
                    var denom = decimal.Parse(Console.ReadLine() ?? "0");

                    // Validate if the denomination exists in the available denominations
                    var validDenominations = CurrencyConfig.Instance.GetDenominations();

                    if (!validDenominations.Contains(denom))
                    {
                        Console.WriteLine("Invalid denomination. Please enter a valid denomination.");
                        continue;
                    }

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
                    _logger.LogWarning($"Invalid input. Error: {ex.Message}");
                }

            return paymentInDenominations;
        }
    }