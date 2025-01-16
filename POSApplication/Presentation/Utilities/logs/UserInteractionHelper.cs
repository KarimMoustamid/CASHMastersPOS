using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using POSApplication.Data;
using POSApplication.Data.Models;
using POSApplication.Presentation.Utilities.logs;

public class UserInteractionHelper : IUserInteractionHelper
{
    private readonly ILogger<UserInteractionHelper> _logger;
    private readonly ICurrencyConfig _currencyConfig;

    // Constructor to inject dependencies
    public UserInteractionHelper(ILogger<UserInteractionHelper> logger, ICurrencyConfig currencyConfig)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currencyConfig = currencyConfig ?? throw new ArgumentNullException(nameof(currencyConfig));
    }

    public void CurrencyDenominations()
    {
        var denominations = _currencyConfig.GetDenominations();
        ConsoleHelper.LogInfo("Available denominations:\n");
        foreach (var denom in denominations)
        {
            Console.WriteLine($"- {denom:C}\n");
        }
    }

    public void DisplayAvailableCurrencies()
    {
        try
        {
            var availableCurrencies = _currencyConfig.GetAvailableCurrencies();
            _logger.LogInformation("Displaying available currencies:");

            foreach (var currency in availableCurrencies)
            {
                Console.WriteLine($"- {currency?.Country} ({currency?.CurrencyCode})");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while displaying available currencies.");
            throw;
        }
    }

    public T GetInput<T>(string prompt, string errorMessage)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            try
            {
                return (T) Convert.ChangeType(input, typeof(T));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, errorMessage);
            }
        }
    }

    public Dictionary<decimal, int> CollectPaymentInput()
    {
        var paymentInDenominations = new Dictionary<decimal, int>();

        while (true)
        {
            try
            {
                Console.Write("Denomination: ");
                if (!decimal.TryParse(Console.ReadLine(), out var denom))
                {
                    _logger.LogWarning("Invalid denomination input.");
                    Console.WriteLine("Invalid denomination. Please enter a valid number.");
                    continue;
                }

                // Validate if the denomination exists in the available denominations
                var validDenominations = _currencyConfig.GetDenominations();

                if (!validDenominations.Contains(denom))
                {
                    _logger.LogWarning("Invalid denomination entered: {Denomination}", denom);
                    Console.WriteLine("Invalid denomination. Please enter a valid denomination.");
                    continue;
                }

                Console.Write("Count: ");
                if (!int.TryParse(Console.ReadLine(), out var count) || count < 0)
                {
                    _logger.LogWarning("Invalid count input.");
                    Console.WriteLine("Invalid count. Please enter a positive integer.");
                    continue;
                }

                if (paymentInDenominations.ContainsKey(denom))
                    paymentInDenominations[denom] += count;
                else
                    paymentInDenominations[denom] = count;

                Console.Write("Add another denomination? (y/n): ");
                var addMore = Console.ReadLine()?.ToLower();
                if (addMore != "y")
                    break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error while collecting payment input.");
                Console.WriteLine("Invalid input. Please try again.");
            }
        }

        return paymentInDenominations;
    }
}