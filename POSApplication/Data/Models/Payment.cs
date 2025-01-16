namespace POSApplication.Data.Models
{
    // Represents a payment made in a transaction.
    // This model encapsulates payment details, including total amount paid and the breakdown by denominations,
    // simplifying how payment information is managed and extended.
    public class Payment
    {
        // The total amount paid by the customer.
        // This value represents the sum of the entire payment made across all denominations.
        public decimal TotalPaid { get; set; }

        // A dictionary representing the breakdown of the payment by denominations.
        // Key: The denomination value (e.g., 1.00, 0.25).
        // Value: The count of each denomination provided during the payment.
        public Dictionary<decimal, int> Denominations { get; set; } = new();
    }
}