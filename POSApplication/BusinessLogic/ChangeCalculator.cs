namespace POSApplication.BusinessLogic
{
    public class ChangeCalculator
    {
        public static Dictionary<decimal, int> CalculateChange(decimal price, decimal paid, IReadOnlyList<decimal> denominations)
        {
            // Check if the paid amount is less than the price
            if (paid < price)
            {
                throw new ArgumentException("The paid amount must be greater than or equal to price.");
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