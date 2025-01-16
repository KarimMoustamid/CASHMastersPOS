namespace POSApplication.BusinessLogic.Utilities.Payments;

// Utility class that provides methods related to payment calculations.
public class Calculate
{
    // Calculates the total amount paid based on a dictionary of payment denominations.
    // Parameters:
    // - paymentInDenominations: A dictionary where:
    //   - The key represents the denomination (e.g., 1.00, 0.25).
    //   - The value represents the count of each denomination provided in the payment.
    // Returns:
    // - The total payment amount as a decimal, calculated by summing up
    //   (denomination * count) for all dictionary entries.
    public decimal TotalPaid(Dictionary<decimal, int> paymentInDenominations)
    {
        // Uses LINQ's Sum() method to calculate the total:
        // For each entry, multiplies the denomination (key) by the count (value),
        // and sums the results from all entries.
        return paymentInDenominations.Sum(entry => entry.Key * entry.Value);
    }
}