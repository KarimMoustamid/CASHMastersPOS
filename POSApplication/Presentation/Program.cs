using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using POSApplication.BusinessLogic;
using POSApplication.BusinessLogic.Services;
using POSApplication.BusinessLogic.Utilities.logs;
using POSApplication.BusinessLogic.Utilities.Payments;
using POSApplication.Data.Models;


// Manually create the logger factory
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole(); // Adds logging to the console
});

// Create the logger instance
var logger = new Logger(loggerFactory.CreateLogger<Logger>());

try
{
    logger.LogInformation("Welcome to the CASH Masters POS System!\n");

    // Display available countries
    Display.DisplayAvailableCurrencies(logger);


    // Let the user choose a country
    Console.Write("\nEnter the currency Code for the currency configuration: ");
    var currencyCode = InputValidator.ValidateInput(logger, input => !string.IsNullOrWhiteSpace(input), "Invalid currency code! Please try again.");

    // Set the selected currency
    CurrencyConfig.Instance.SetCurrency(currencyCode);
    logger.LogInformation("Currency loaded successfully!");


    var denominations = CurrencyConfig.Instance.GetDenominations();
    logger.LogInformation("Available Denominations:");
    foreach (var denom in denominations)
    {
        Console.WriteLine($"- {denom:C}\n");
    }
    // Collect Input for change calculation
    var price = Display.GetInput<decimal>(logger, "Enter the price of the item(s): ", "Invalid price! Please enter a valid decimal value.");

    Console.Write("Register the payment breakdown by denomination and coins: \n");
    var paymentInDenominations = Display.CollectPaymentInput(logger);

    // Calculate the total paid
    var calculate = new Calculate();
    var totalPaid = calculate.TotalPaid(paymentInDenominations);

    var payment = new Payment
    {
        TotalPaid = totalPaid,
        Denominations = paymentInDenominations
    };


    logger.LogInformation($"Total amount paid: {totalPaid:C}");

    // Calculate change
    var calculator = new ChangeCalculator();
    var change = calculator.CalculateChange(price, payment, currencyCode);
    logger.LogInformation("Change to return:");

    foreach (var item in change.Denominations)
    {
        Console.WriteLine($"{item.Value} x {item.Key:C}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}