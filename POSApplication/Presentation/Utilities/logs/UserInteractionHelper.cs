// Using directive for logging interface to handle logging within the application.
using Microsoft.Extensions.Logging;
// Using directive to access data-related functionalities and models within the application's data layer.
using POSApplication.Data;
// Using directive for custom logging utilities specific to the presentation layer of the application.
using POSApplication.Presentation.Utilities.logs;

// Helper class to interact with the user and handle inputs related to currency, payments, and configurations.
// This class simplifies user interactions and ensures proper logging and validation during operations.
public class UserInteractionHelper : IUserInteractionHelper
{
    private readonly ILogger<UserInteractionHelper> _logger; // Logger for logging information, warnings, and errors.
    private readonly ICurrencyConfig _currencyConfig; // Dependency to manage currency configurations.

    // Constructor to inject dependencies.
    // Parameters:
    // - logger: Tool for logging information and issues during runtime.
    // - currencyConfig: The interface to handle currency-specific operations like fetching denominations.
    public UserInteractionHelper(ILogger<UserInteractionHelper> logger, ICurrencyConfig currencyConfig)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currencyConfig = currencyConfig ?? throw new ArgumentNullException(nameof(currencyConfig));
    }

    // Displays available currency denominations to the console.
    // Retrieves valid denominations for the current currency and prints them in a formatted way.
    public void CurrencyDenominations()
    {
        var denominations = _currencyConfig.GetDenominations();
        ConsoleHelper.LogInfo("Available denominations:\n");
        foreach (var denom in denominations)
        {
            Console.WriteLine($"- {denom:C}\n");
        }
    }

    // Displays available currencies to the console.
    // Retrieves a list of currencies and prints their associated country and currency code.
    // Logs errors if an exception occurs during the process.
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

    // Generic method to prompt and parse user input.
    // Continuously asks the user to input a value of type T until a valid response is provided.
    // Parameters:
    // - prompt: The message to display to the user.
    // - errorMessage: The message to log and display in case of input errors.
    // Returns:
    // - The input value converted to type T.
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

    // Collects payment information from the user in the form of denominations and counts.
    // Repeatedly prompts the user to enter payment details, such as denomination values and their counts,
    // validating each entry against the available currency denominations.
    // Returns:
    // - A dictionary where the key is the denomination and the value is the count entered by the user.
    public Dictionary<decimal, int> CollectPaymentInput()
    {
        var paymentInDenominations = new Dictionary<decimal, int>();

        while (true)
        {
            try
            {

                // Prompt the user to input a denomination value and try to parse it as a decimal.
                // If parsing fails, log a warning and ask the user again until a valid input is provided.
                Console.Write("Enter the denomination given by the customer: ");
                if (!decimal.TryParse(Console.ReadLine(), out var denom))
                {
                    _logger.LogWarning("Invalid denomination input.");
                    Console.WriteLine("Invalid denomination. Please enter a valid number.");
                    continue;
                }

                // Validate if the denomination exists in the available denominations.
                var validDenominations = _currencyConfig.GetDenominations();

                if (!validDenominations.Contains(denom))
                {
                    _logger.LogWarning("Invalid denomination entered: {Denomination}", denom);
                    Console.WriteLine("Invalid denomination. Please enter a valid denomination.");
                    continue;
                }


                string denominationType = denom > 20 ? "bill" : "coin";
                Console.Write($"How many {denom} {denominationType}s is the customer giving? Enter count: ");
                if (!int.TryParse(Console.ReadLine(), out var count) || count < 0)
                {
                    _logger.LogWarning("Invalid count input.");
                    Console.WriteLine("Invalid count. Please enter a positive integer.");
                    continue;
                }

                // Add or update count for the entered denomination.
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