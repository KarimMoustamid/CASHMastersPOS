namespace POSApplication.BusinessLogic
{
    using Data.Models;

    // Interface for defining the contract of a change calculator.
    // Allows the calculation of change for a transaction based on the price, payment, and currency details.
    public interface IChangeCalculator
    {
        // Method signature for calculating the change.
        // Parameters:
        // - price: The cost of the item or transaction.
        // - payment: The details of the payment made, including denominations and total amount.
        // - currencyCode: The currency code (e.g., USD, MXN) used for the transaction.
        // Returns:
        // - A Change object containing the calculated change details, including total change and its breakdown.
        Change CalculateChange(decimal price, Payment payment, string currencyCode);
    }
}