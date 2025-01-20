namespace POSApplication.Presentation
{
    // Importing required namespaces
    using BusinessLogic; // Encapsulates core business logic.
    using BusinessLogic.config; // Manages configuration-related operations in the business logic layer.
    using BusinessLogic.Utilities; // Includes general-purpose utilities for the application.
    using BusinessLogic.Utilities.Payments; // Contains payment-related utilities.
    using Data; // Handles data access and manipulation.
    using Data.Models; // Defines the data models used in the application.
    using Microsoft.Extensions.Logging; // Provides logging support.
    using Utilities.logs; // Includes custom logging utilities.

    // The main class responsible for running the application's core processes.
    public class ApplicationRunner
    {
        // Private readonly fields for injected services
        private readonly ILogger<ApplicationRunner> _logger; // Handles logging for application operations.
        private readonly ICurrencyConfig _currencyConfig; // Responsible for currency configuration.
        private readonly UserInteractionHelper _userInteractionHelper; // Handles user interactions (input/output).
        private readonly ChangeCalculator _changeCalculator; // Manages the logic for calculating change.

        // Constructor to initialize dependencies using Dependency Injection
        public ApplicationRunner(
            ILogger<ApplicationRunner> logger, // Logger specifically for ApplicationRunner.
            ICurrencyConfig currencyConfig, // Implementation of currency configuration.
            UserInteractionHelper userInteractionHelper, // Utility class for user interactions.
            ChangeCalculator changeCalculator // Responsible for change calculation logic.
        )
        {
            // Assigning injected services to private fields
            _logger = logger;
            _currencyConfig = currencyConfig;
            _userInteractionHelper = userInteractionHelper;
            _changeCalculator = changeCalculator;
        }

        // The main method responsible for running the point-of-sale (POS) logic
        public void Run()
        {
            try
            {
                // Pre-configuration for the currency system before the application starts
                PreConfigureCurrency();

                // Display welcome message to the user
                ConsoleHelper.LogSuccess("Welcome to the CASH Masters POS System!\n");

                // Displays available currency denominations to the console
                _userInteractionHelper.CurrencyDenominations();

                // Ask the user to input the total price of the items
                var price = _userInteractionHelper.GetInput<decimal>("Enter the price of the item(s): ", "Invalid price!");

                decimal totalPaid = 0.0M; // Tracks the cumulative payment made by the user.
                var totalChange = -1.0M; // Tracks the total change to be returned.

                // Loop until the full payment is made
                while (totalChange < 0)
                {
                    // Prompt user to input payment denominations
                    var paymentInDenominations = _userInteractionHelper.CollectPaymentInput();
                    totalPaid += new Calculate().TotalPaid(paymentInDenominations); // Calculating cumulative payments.

                    // Create a Payment object to store payment details
                    var payment = new Payment
                    {
                        TotalPaid = totalPaid,
                        Denominations = paymentInDenominations
                    };

                    // Log the total payment received so far
                    ConsoleHelper.LogSuccess($"\nTotal amount paid so far: {totalPaid:C}");

                    // Calculate the change based on the price, payment, and currency
                    var change = _changeCalculator.CalculateChange(price, payment, _currencyConfig.GetCurrencyCode());
                    totalChange = change.TotalChange;

                    // Check if there is insufficient payment, no change, or remaining change
                    if (totalChange < 0)
                    {
                        // Prompt the user to pay the remaining amount
                        ConsoleHelper.LogError($"Insufficient payment. Please pay additional {Math.Abs(totalChange):C}");
                    }
                    else if (totalChange == 0)
                    {
                        // Inform the user that no change is due
                        ConsoleHelper.LogError("No change to return.");
                    }
                    else
                    {
                        // Display the change to be returned to the user
                        ConsoleHelper.LogWarning("\nChange to return:");
                        change.DisplayChange(); // Use an extension method for displaying change.
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during application execution
                _logger.LogError(ex, "An error occurred during application execution.");
            }
        }

        // A private method to pre-configure the currency settings for the application
        private void PreConfigureCurrency()
        {
            try
            {
                // Retrieve the currency configuration file path dynamically
                var currencyFilePath = ConfigLoader.GetCurrencyFilePath();

                // Initialize the currency configuration using the file path
                _currencyConfig.Initialize(currencyFilePath);


                // Refer to the "Manage and Add a New Currency" section in the README file for detailed steps
                // to configure and enable support for additional currencies in the application.
                // To enable a different currency by default, replace CurrencyConstants.USD with your desired currency constant.
                // For example, use CurrencyConstants.MXN for Mexican Peso.
                // _currencyConfig.SetCurrency(CurrencyConstants.MXN);

                // Set the default currency (e.g., USD)
                _currencyConfig.SetCurrency(CurrencyConstants.USD);

                // Log success message with the configured currency code
                _logger.LogInformation("Currency {CurrencyCode} was configured successfully.\n",
                    _currencyConfig.GetCurrency()?.CurrencyCode);
            }
            catch (Exception ex)
            {
                // Log any errors that occur during currency configuration
                _logger.LogError(ex, "An error occurred during currency configuration.");
                // Rethrow the exception to ensure proper error handling in the calling code
                throw;
            }
        }
    }
}