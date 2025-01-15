namespace POSApplication.BusinessLogic
{
    public class ChangeCalculator
    {
        public static Dictionary<decimal, int> CalculateChange(decimal price, Dictionary<decimal, int> payment, string currencyCode)
        {
            var currency = CurrencyConfig.Instance.GetCurrency();
            var denominations = currency?.Denominations;

            // Calculate total payment
            var totalPaid = payment.Sum(p => p.Key * p.Value);

            if (totalPaid < price)
                throw new ArgumentException("Insufficient payment provided.");

            var changeToReturn = totalPaid - price;
            var change = new Dictionary<decimal, int>();

            foreach (var denom in denominations)
            {
                if (payment.ContainsKey(denom)) continue; // Skip denominations already used in payment.

                var count = (int)(changeToReturn / denom);
                if (count > 0)
                {
                    change[denom] = count;
                    changeToReturn -= count * denom;
                }

                // Avoid floating-point precision issues
                changeToReturn = Math.Round(changeToReturn, 2);
            }

            if (changeToReturn > 0)
                throw new InvalidOperationException("Cannot provide exact change with available denominations.");

            return change;
        }
        public static Dictionary<decimal, int> CalculateChangeLegacy(decimal price, decimal paid, IReadOnlyList<decimal> denominations)
        {
            // Check if the paid amount is less than the price
            if (paid < price)
            {
                throw new ArgumentException("The paid amount must be greater than or equal to price.");
            }

            // Handle the case where the paid amount is exactly equal to the price
            if (paid == price)
            {
                Console.WriteLine("No change required: Paid amount is equal to the price.");
                return new Dictionary<decimal, int>(); // Return an empty dictionary since no change is needed
            }

            // Calculate the total change to return to the customer
            var changeToReturn = paid - price;

            // Initialize a dictionary to store the change breakdown
            var change = new Dictionary<decimal, int>();

            // Loop through each denomination to calculate the change
            foreach (var denom in denominations)
            {
                // Calculate how many of the current denomination can be used
                var count = (int) (changeToReturn / denom);

                // If the count is greater than zero, add it to the change dictionary
                if (count > 0)
                {
                    change[denom] = count; // Add the denomination and count to the dictionary
                    changeToReturn -= count * denom; // Deduct the value of this denomination from the change to return
                }

                // Round the remaining change to avoid floating-point precision issues
                changeToReturn = Math.Round(changeToReturn, 2);
            }

            // Return the calculated change
            return change;
        }
    }
}