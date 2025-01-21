namespace POSApplication.BusinessLogic
{
    using Data;
    using Data.Models;
    using Presentation.Utilities.logs;

    // Class responsible for calculating the change given a price, payment, and currency.
    // Implements the IChangeCalculator interface.
    public class ChangeCalculator : IChangeCalculator
    {
        // Dependency field to hold the currency configuration, injected via the constructor.
        private readonly ICurrencyConfig _currencyConfig;


        // Constructor for dependency injection.
        // Accepts an implementation of ICurrencyConfig to provide currency details such as denominations.
        public ChangeCalculator(ICurrencyConfig currencyConfig)
        {
            // Throws an exception if the provided configuration is null to avoid runtime errors.
            _currencyConfig = currencyConfig ?? throw new ArgumentNullException(nameof(currencyConfig));
        }

        // Main method to calculate the change to be returned in a transaction.
        // Takes the price of the item, payment details, and currency code as inputs.
        public Change CalculateChange(decimal price, Payment payment, string currencyCode)
        {
            // Validate the provided inputs.

            // 1. Ensure the currency code is not null, empty, or whitespace.
            if (string.IsNullOrWhiteSpace(currencyCode))
                throw new ArgumentException("Currency code cannot be null or empty.", nameof(currencyCode));

            // 2. Ensure the price is greater than 0 (zero).
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            // Use helper classes to validate payment and price values.
            InputValidator.ValidatePrice(price);
            InputValidator.ValidatePayment(payment, _currencyConfig.GetDenominations());

            // Get currency details from the configuration and confirm it matches the provided currency code.
            var currency = _currencyConfig.GetCurrency();
            if (currency == null || currency.CurrencyCode != currencyCode)
                throw new InvalidOperationException($"The currency code '{currencyCode}' is not currently loaded.");

            // Ensure the currency has valid denominations defined.
            var denominations = currency.Denominations;
            if (denominations == null || !denominations.Any())
                throw new InvalidOperationException($"The currency '{currencyCode}' does not have valid denominations.");

            // Calculate the total change to return by subtracting the price from the total payment provided.
            var totalPaid = payment.TotalPaid;
            var changeToReturn = totalPaid - price;

            // If the total payment is insufficient, throw an error.
            if (changeToReturn < 0)
                ConsoleHelper.LogError("The payment provided is insufficient. Please collect the remaining amount from the client.");


            // Calculate the denominations to return as change using helper logic.
            var changeDenominations = CalculateChangeDenominations(changeToReturn, denominations);

            // Return the calculated change detail, including total change and the denomination breakdown.
            return new Change
            {
                Denominations = changeDenominations,
                TotalChange = totalPaid - price
            };
        }

        // Helper method to calculate the specific denominations to return based on available denominations.
        // Takes the total change amount and the list of available denominations as inputs.
        private Dictionary<decimal, int> CalculateChangeDenominations(decimal changeToReturn, IEnumerable<decimal> denominations)
        {
            // Dictionary to store the breakdown of change denominations and their respective counts.
            var changeDenominations = new Dictionary<decimal, int>();

            // Iterate over the denominations in descending order (largest to smallest).
            foreach (var denom in denominations.OrderByDescending(d => d))
            {
                // Stop processing once the entire change has been calculated.
                if (changeToReturn <= 0) break;

                // Calculate how many units of the current denomination are required for the change.
                var count = (int) (changeToReturn / denom);
                if (count > 0)
                {
                    // Add the denomination and count to the result and deduct their value from the change.
                    changeDenominations[denom] = count;
                    changeToReturn -= count * denom;
                }

                // Round the remaining change to avoid floating-point precision errors.
                changeToReturn = Math.Round(changeToReturn, 2);
            }

            // If there is still leftover change that couldn't be returned, throw an error.
            if (changeToReturn > 0)
                throw new InvalidOperationException("Unable to provide exact change with available denominations.");

            // Return the calculated denominations and their counts.
            return changeDenominations;
        }
    }
}