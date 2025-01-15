namespace POSApplication.BusinessLogic
{
    public class ChangeCalculator
    {
        public static Dictionary<decimal, int> CalculateChange(decimal price, decimal paid, IReadOnlyList<decimal> denominations)
        {
            if (paid < price)
            {
                throw new ArgumentException("The paid amount must be greater than or equal to price.");
            }

            var changeToReturn = paid - price;
            var change = new Dictionary<decimal, int>();

            return change;
        }
    }
}