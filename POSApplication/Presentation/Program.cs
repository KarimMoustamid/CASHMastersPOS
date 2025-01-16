using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using POSApplication.BusinessLogic;
using POSApplication.BusinessLogic.config;
using POSApplication.BusinessLogic.Utilities;
using POSApplication.BusinessLogic.Utilities.Payments;
using POSApplication.Data;
using POSApplication.Data.Models;
using POSApplication.Presentation.Utilities.logs;

#region DI
// Dependency Injection (DI) configuration
var services = new ServiceCollection();

// Register services and dependencies for the application
services.AddSingleton<ICurrencyConfig, CurrencyConfig>(); // Currency configuration service
services.AddTransient<ChangeCalculator>(); // Service to calculate change
services.AddLogging(config =>
{
    config.ClearProviders(); // Clears default logging providers
    config.AddConsole();     // Adds console-based logging
});
services.AddTransient<UserInteractionHelper>(); // Utilities for user input and interaction

// Build the service provider to resolve dependencies
var serviceProvider = services.BuildServiceProvider();

// Resolve dependencies and log their status
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Logger successfully resolved.");

var userInteractionHelper = serviceProvider.GetRequiredService<UserInteractionHelper>();
logger.LogInformation("UserInteractionHelper successfully resolved.");

var currencyConfig = serviceProvider.GetRequiredService<ICurrencyConfig>();
logger.LogInformation("Currency configuration service successfully resolved.");

var changeCalculator = serviceProvider.GetRequiredService<ChangeCalculator>();
logger.LogInformation("ChangeCalculator service successfully resolved.");

// Log application initialization
logger.LogInformation("Starting application configuration...");
#endregion

#region ApplicationPreConfiguration
try
{
    // Pre-configuration for currency settings
    // Load the currency configuration file path
    string currencyFilePath = ConfigLoader.GetCurrencyFilePath();

    // Initialize the currency configuration from the file
    currencyConfig.Initialize(currencyFilePath);

    // Set a default currency for the application (USD in this case)
    currencyConfig.SetCurrency(CurrencyConstants.USD);

    logger.LogInformation("Currency {CurrencyCode} was configured successfully.\n\n", currencyConfig.GetCurrency()?.CurrencyCode);
}
catch (Exception ex)
{
    // Log any error during currency pre-configuration
    logger.LogError(ex, "An error occurred during currency configuration.");
}
#endregion

#region Console
try
{
    // Welcome message
    ConsoleHelper.LogSuccess("Welcome to the CASH Masters POS System!\n");

    // Display available currency denominations
    userInteractionHelper.CurrencyDenominations();

    // Prompt user to enter the price of the item(s)
    var price = userInteractionHelper.GetInput<decimal>("Enter the price of the item(s): ", "Invalid price! Please enter a valid decimal value.");

    // Prompt user to register payment denominations
    ConsoleHelper.LogWarning("\nPlease register the payment by entering the denominations and coins: \n");
    var paymentInDenominations = userInteractionHelper.CollectPaymentInput();

    // Calculate the total paid using the entered denominations
    var calculate = new Calculate();
    var totalPaid = calculate.TotalPaid(paymentInDenominations);

    // Create a payment object from the input
    var payment = new Payment
    {
        TotalPaid = totalPaid,
        Denominations = paymentInDenominations
    };

    // Display the total amount paid
    ConsoleHelper.LogSuccess($"\nTotal amount paid: {totalPaid:C}");

    // Calculate the change to be returned
    var calculator = new ChangeCalculator(currencyConfig);
    var currencyCode = currencyConfig.GetCurrency()?.CurrencyCode;
    var change = calculator.CalculateChange(price, payment, currencyCode); // Calculate change based on price, payment, and currency code

    // Display the change or log an error if no change is required
    if (change.TotalChange == 0)
    {
        ConsoleHelper.LogError("No change to return.");
    }
    else
    {
        ConsoleHelper.LogWarning("\nChange to return:");
        foreach (var item in change.Denominations)
        {
            Console.WriteLine($"{item.Value} x {item.Key:C}"); // Display change denominations and counts
        }
    }
}
catch (Exception ex)
{
    // Log any exception during the main application process
    logger.LogError(ex.Message);
}
#endregion