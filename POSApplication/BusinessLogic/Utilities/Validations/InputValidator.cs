using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using POSApplication.Data.Models;

public static class InputValidator
{
    // Static field for logger
    private readonly static ILogger _logger;

    static InputValidator()
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(options =>
            {
                options.FormatterName = "simple"; // Use the custom formatter
            });

            builder.AddConsoleFormatter<SimpleConsoleFormatter, ConsoleFormatterOptions>();
        });

        _logger = loggerFactory.CreateLogger("InputValidator"); // Generic logger for a static class
    }

    public static void ValidatePayment(Payment payment, List<decimal> validDenominations)
    {
        foreach (var denom in payment.Denominations.Keys)
            if (!validDenominations.Contains(denom))
                throw new ArgumentException($"Invalid denomination: {denom}");

        if (payment.TotalPaid < 0) throw new ArgumentException("Total paid cannot be negative.");
    }

    public static void ValidatePrice(decimal price)
    {
        if (price < 0) throw new ArgumentException("Price cannot be negative.");
    }

    public static string ValidateInput(Func<string, bool> validation, string errorMessage)
    {
        while (true)
        {
            var input = Console.ReadLine()?.Trim();
            if (validation(input)) return input;

            _logger.LogWarning(errorMessage);
        }
    }
}