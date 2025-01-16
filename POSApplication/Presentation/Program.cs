using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using POSApplication.BusinessLogic;
using POSApplication.BusinessLogic.config;
using POSApplication.BusinessLogic.Utilities;
using POSApplication.BusinessLogic.Utilities.Payments;
using POSApplication.Data.Models;


using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Register any additional services if needed
    })
    .ConfigureLogging(logging =>
    {
        // Clear all default logging providers (e.g., Debug and Console)
        logging.ClearProviders();

        // Add console logging with the custom formatter
        logging.AddConsole(options =>
        {
            options.FormatterName = "Simple"; // Match the formatter name
        });

        // Register the custom formatter
        logging.AddConsoleFormatter<SimpleConsoleFormatter, ConsoleFormatterOptions>();
    })
    .Build();

// Resolve the logger and write a test message
var logger = host.Services.GetRequiredService<ILogger<Program>>();

try
{
// Load currency file path from configuration
    string currencyFilePath = ConfigLoader.GetCurrencyFilePath();
    // Initialize the CurrencyConfig instance
    CurrencyConfig.Instance.Initialize(currencyFilePath);

    Console.WriteLine("Currency configuration loaded successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

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
    CurrencyConfig.Instance.SetCurrency(currencyCode);
    logger.LogInformation("\nCurrency loaded successfully!");


    var denominations = CurrencyConfig.Instance.GetDenominations();
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
    Console.WriteLine(ex.Message);
}