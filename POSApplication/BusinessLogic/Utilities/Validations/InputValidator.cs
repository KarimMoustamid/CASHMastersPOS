using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using POSApplication.Data.Models;
using POSApplication.Presentation.Utilities.logs;

public static class InputValidator
{
    // Static field for logger.
    // This logger is used to log messages such as warnings during validation.
    private readonly static ILogger _logger;


    // Static constructor to initialize the logger instance.
    // A logger factory is created and configured to include console logging.
    static InputValidator()
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            // Configures the logger to output logs to the console.
            builder.AddConsole();
        });

        // Creates a logger instance specifically for the "InputValidator" class.
        _logger = loggerFactory.CreateLogger("InputValidator");
    }


    // Validates the `Payment` object against valid denominations and other rules.
    public static void ValidatePayment(Payment payment, IReadOnlyList<decimal> validDenominations)
    {
        // Verify if each key in the payment's denominations is within the valid denominations list.
        foreach (var denom in payment.Denominations.Keys)
            if (!validDenominations.Contains(denom))
                throw new ArgumentException($"Invalid denomination: {denom}"); // Throws exception if invalid denomination is found.

        // Ensure the total paid amount is not negative.
        if (payment.TotalPaid < 0) throw new ArgumentException("Total paid cannot be negative.");
    }


    // Validates the price to ensure it's not negative
    public static void ValidatePrice(decimal price)
    {
        // Throws an exception if the price is less than zero.
        if (price < 0) throw new ArgumentException("Price cannot be negative.");
    }

    // Reads and validates user input from the console based on custom validation logic.
    // Logs a warning whenever the validation fails and continues asking the user until valid input is provided.
    public static string ValidateInput(Func<string, bool> validation, string errorMessage)
    {
        while (true)
        {
            // Reads input from the console and trims any leading/trailing whitespace.
            var input = Console.ReadLine()?.Trim();

            // If the input passes the validation function, return it.
            if (input != null && validation(input)) return input;

            // Logs a warning with the provided error message if validation fails.
            _logger.LogWarning(errorMessage);
            ConsoleHelper.LogError(errorMessage);
        }
    }
}