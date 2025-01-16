namespace POSApplication.BusinessLogic
{
    using Data;
    using Data.Models;
    public class ChangeCalculator
    {
        private readonly ICurrencyConfig _currencyConfig;

        // Constructor for dependency injection
        public ChangeCalculator(ICurrencyConfig currencyConfig)
        {
            _currencyConfig = currencyConfig ?? throw new ArgumentNullException(nameof(currencyConfig));
        }

        public Change CalculateChange(decimal price, Payment payment, string currencyCode)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(currencyCode))
                throw new ArgumentException("Currency code cannot be null or empty.", nameof(currencyCode));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            InputValidator.ValidatePrice(price);
            InputValidator.ValidatePayment(payment, _currencyConfig.GetDenominations());

            // Get currency details and validate
            var currency = _currencyConfig.GetCurrency();
            if (currency == null || currency.CurrencyCode != currencyCode)
                throw new InvalidOperationException($"The currency code '{currencyCode}' is not currently loaded.");

            var denominations = currency.Denominations;
            if (denominations == null || !denominations.Any())
                throw new InvalidOperationException($"The currency '{currencyCode}' does not have valid denominations.");

            // Calculate the change to return
            var totalPaid = payment.TotalPaid;
            var changeToReturn = totalPaid - price;

            if (changeToReturn < 0)
                throw new ArgumentException("Insufficient payment provided.");

            var changeDenominations = CalculateChangeDenominations(changeToReturn, denominations);

            return new Change
            {
                Denominations = changeDenominations,
                TotalChange = totalPaid - price
            };
        }

        private Dictionary<decimal, int> CalculateChangeDenominations(decimal changeToReturn, IEnumerable<decimal> denominations)
        {
            var changeDenominations = new Dictionary<decimal, int>();

            foreach (var denom in denominations.OrderByDescending(d => d))
            {
                if (changeToReturn <= 0) break;

                var count = (int) (changeToReturn / denom);
                if (count > 0)
                {
                    changeDenominations[denom] = count;
                    changeToReturn -= count * denom;
                }

                // Avoid floating-point errors
                changeToReturn = Math.Round(changeToReturn, 2);
            }

            if (changeToReturn > 0)
                throw new InvalidOperationException("Unable to provide exact change with available denominations.");

            return changeDenominations;
        }
    }
}