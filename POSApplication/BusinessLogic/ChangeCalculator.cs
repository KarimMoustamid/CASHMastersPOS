namespace POSApplication.BusinessLogic
{
    using Data.Models;

    public class ChangeCalculator
    {
        public Change CalculateChange(decimal price, Payment payment, string currencyCode)
        {
            var currency = CurrencyConfig.Instance.GetCurrency();
            var denominations = currency?.Denominations;

            InputValidator.ValidatePrice(price);
            InputValidator.ValidatePayment(payment, denominations);

            // Calculate total payment
            var totalPaid = payment.TotalPaid;
            var changeToReturn = totalPaid - price;

            if (changeToReturn < 0)
                throw new ArgumentException("Insufficient payment provided.");

            var changeDenominations = new Dictionary<decimal, int>();
            foreach (var denom in denominations)
            {
                var count = (int) (changeToReturn / denom);
                if (count > 0)
                {
                    changeDenominations[denom] = count;
                    changeToReturn -= count * denom;
                }

                changeToReturn = Math.Round(changeToReturn, 2); // Avoid floating-point errors
            }

            return new Change
            {
                Denominations = changeDenominations,
                TotalChange = totalPaid - price
            };
        }
    }
}