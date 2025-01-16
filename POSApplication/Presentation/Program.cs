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
var services = new ServiceCollection();
services.AddSingleton<ICurrencyConfig, CurrencyConfig>();
services.AddTransient<ChangeCalculator>();
services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
});

services.AddTransient<UserInteractionHelper>();

var serviceProvider = services.BuildServiceProvider();

// Resolve Dependencies :
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Logger successfully resolved.");

var userInteractionHelper = serviceProvider.GetRequiredService<UserInteractionHelper>();
logger.LogInformation("UserInteractionHelper successfully resolved.");

var currencyConfig = serviceProvider.GetRequiredService<ICurrencyConfig>();
logger.LogInformation("Currency configuration service successfully resolved.");

var changeCalculator = serviceProvider.GetRequiredService<ChangeCalculator>();
logger.LogInformation("changeCalculator configuration service successfully resolved.");


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

    logger.LogInformation("Currency {CurrencyCode} was configured successfully.\n\n", currencyConfig.GetCurrency()?.CurrencyCode);

}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during currency configuration.");
}
#endregion

#region Console
try
{
    ConsoleHelper.LogSuccess("Welcome to the CASH Masters POS System!\n");

    userInteractionHelper.CurrencyDenominations();

    // Collect Input for change calculation
    var price = userInteractionHelper.GetInput<decimal>("Enter the price of the item(s): ", "Invalid price! Please enter a valid decimal value.");

    ConsoleHelper.LogWarning("\nPlease register the payment by entering the denominations and coins: \n");
    var paymentInDenominations = userInteractionHelper.CollectPaymentInput();

    // Calculate the total paid
    var calculate = new Calculate();
    var totalPaid = calculate.TotalPaid(paymentInDenominations);

    var payment = new Payment
    {
        TotalPaid = totalPaid,
        Denominations = paymentInDenominations
    };


    ConsoleHelper.LogSuccess($"\nTotal amount paid: {totalPaid:C}");

    // Calculate change
    var calculator = new ChangeCalculator(currencyConfig);
    var currencyCode = currencyConfig.GetCurrency()?.CurrencyCode;
    var change = calculator.CalculateChange(price, payment, currencyCode);

    if (change.TotalChange == 0)
    {
        ConsoleHelper.LogError("No change to return.");
    }
    else
    {
        ConsoleHelper.LogWarning("\nChange to return:");
        foreach (var item in change.Denominations)
        {
            Console.WriteLine($"{item.Value} x {item.Key:C}");
        }
    }
}
catch (Exception ex)
{
    logger.LogError(ex.Message);
}
#endregion