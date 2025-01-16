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


#region DI_Configuration
var services = new ServiceCollection();
services.AddSingleton<ICurrencyConfig, CurrencyConfig>();
services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole(options =>
    {
        options.FormatterName = "Simple"; // Match the formatter name
    });

    config.AddConsoleFormatter<SimpleConsoleFormatter, ConsoleFormatterOptions>();
});

var serviceProvider = services.BuildServiceProvider();

// Resolve Dependencies :
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Logger successfully resolved.");

var currencyConfig = serviceProvider.GetRequiredService<ICurrencyConfig>();
logger.LogInformation("Currency configuration service successfully resolved.");

// Additional initialization or usage (if applicable)
logger.LogInformation("Starting application configuration...");
#endregion

#region ApplicationPreConfiguration
try
{
    // Load currency file path from configuration
    string currencyFilePath = ConfigLoader.GetCurrencyFilePath();
    currencyConfig.Initialize(currencyFilePath);

    // The default currency is set to USD
    // If we want the system to have a preconfigured currency to a target market, we can set it here:
    // Add the currency to CurrencyConfig.json:
    currencyConfig.SetCurrency(CurrencyConstants.USD);

    logger.LogInformation("Currency {CurrencyCode} was configured successfully.", currencyConfig.GetCurrency()?.CurrencyCode);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during currency configuration.");
}
#endregion

#region Console
try
{
    logger.LogInformation("Welcome to the CASH Masters POS System!\n");

    // Display available countries
    Display.DisplayAvailableCurrencies();


    // Let the user choose a country
    Console.Write("\nEnter the currency Code for the currency configuration: ");

    // Collect existing currency codes
    var validCurrencies = new[] {CurrencyConstants.USD, CurrencyConstants.MXN}; // Add other currencies if needed

    var currencyCode = InputValidator.ValidateInput(
        input =>
            !string.IsNullOrWhiteSpace(input) && validCurrencies.Contains(input.ToUpper()),
        $"Invalid currency code! Please try again. Only valid options are: {CurrencyConstants.USD}, {CurrencyConstants.MXN}."
    );

    // Set the selected currency
    CurrencyConfigLagacy.Instance.SetCurrency(currencyCode);
    logger.LogInformation("\nCurrency loaded successfully!");


    var denominations = CurrencyConfigLagacy.Instance.GetDenominations();
    logger.LogInformation("\nAvailable Denominations:");
    foreach (var denom in denominations)
    {
        Console.WriteLine($"- {denom:C}\n");
    }

    // Collect Input for change calculation
    var price = Display.GetInput<decimal>("Enter the price of the item(s): ", "Invalid price! Please enter a valid decimal value.");

    Console.Write("\nRegister the payment breakdown by denomination and coins: \n");
    var paymentInDenominations = Display.CollectPaymentInput();

    // Calculate the total paid
    var calculate = new Calculate();
    var totalPaid = calculate.TotalPaid(paymentInDenominations);

    var payment = new Payment
    {
        TotalPaid = totalPaid,
        Denominations = paymentInDenominations
    };


    logger.LogInformation($"\nTotal amount paid: {totalPaid:C}");

    // Calculate change
    var calculator = new ChangeCalculator();
    var change = calculator.CalculateChange(price, payment, currencyCode);
    logger.LogInformation("\nChange to return:");

    foreach (var item in change.Denominations)
    {
        Console.WriteLine($"{item.Value} x {item.Key:C}");
    }
}
catch (Exception ex)
{
    logger.LogError(ex.Message);
}
#endregion