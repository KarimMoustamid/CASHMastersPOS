namespace POSApplication.Presentation
{
    using BusinessLogic;
    using BusinessLogic.config;
    using BusinessLogic.Utilities;
    using BusinessLogic.Utilities.Payments;
    using Data;
    using Data.Models;
    using Microsoft.Extensions.Logging;
    using Utilities.logs;

    public class ApplicationRunner
    {
        private readonly ILogger<ApplicationRunner> _logger;
        private readonly ICurrencyConfig _currencyConfig;
        private readonly UserInteractionHelper _userInteractionHelper;
        private readonly ChangeCalculator _changeCalculator;

        public ApplicationRunner(
            ILogger<ApplicationRunner> logger,
            ICurrencyConfig currencyConfig,
            UserInteractionHelper userInteractionHelper,
            ChangeCalculator changeCalculator)
        {
            _logger = logger;
            _currencyConfig = currencyConfig;
            _userInteractionHelper = userInteractionHelper;
            _changeCalculator = changeCalculator;
        }

        public void Run()
        {
            try
            {
                PreConfigureCurrency();

                // Application logic
                ConsoleHelper.LogSuccess("Welcome to the CASH Masters POS System!\n");

                var price = _userInteractionHelper.GetInput<decimal>("Enter the price of the item(s): ", "Invalid price!");

                // Collect payment input from user
                var totalChange = -1.0M;
                decimal totalPaid = 0.0M;

                while (totalChange < 0)
                {
                    // Collect payment input from user
                    var paymentInDenominations = _userInteractionHelper.CollectPaymentInput();
                    totalPaid += new Calculate().TotalPaid(paymentInDenominations);

                    var payment = new Payment
                    {
                        TotalPaid = totalPaid,
                        Denominations = paymentInDenominations
                    };

                    // Log Payment
                    ConsoleHelper.LogSuccess($"\nTotal amount paid so far: {totalPaid:C}");

                    // Calculate change
                    var change = _changeCalculator.CalculateChange(price, payment, _currencyConfig.GetCurrencyCode());
                    totalChange = change.TotalChange;

                    if (totalChange < 0)
                    {
                        ConsoleHelper.LogError($"Insufficient payment. Please pay additional {Math.Abs(totalChange):C}");
                    }
                    else if (totalChange == 0)
                    {
                        ConsoleHelper.LogError("No change to return.");
                    }
                    else
                    {
                        ConsoleHelper.LogWarning("\nChange to return:");
                        change.DisplayChange(); // Extension method
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during application execution.");
            }
        }

        private void PreConfigureCurrency()
        {
            try
            {
                var currencyFilePath = ConfigLoader.GetCurrencyFilePath();
                _currencyConfig.Initialize(currencyFilePath);
                _currencyConfig.SetCurrency(CurrencyConstants.USD);

                _logger.LogInformation("Currency {CurrencyCode} was configured successfully.\n",
                    _currencyConfig.GetCurrency()?.CurrencyCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during currency configuration.");
                throw;
            }
        }
    }
}