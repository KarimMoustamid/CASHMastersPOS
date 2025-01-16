namespace POSApplication.Data.Models
{
    // Represents the details of change to be returned for a transaction.
    // This model simplifies handling and encapsulates information about the change,
    // making it easier to extend and understand.
    public class Change
    {
        // A dictionary that represents the breakdown of change denominations.
        // Key: The denomination value (e.g., 1.00, 0.25).
        // Value: The count of that denomination to be returned as change.
        public Dictionary<decimal, int> Denominations { get; set; } = new();

        // The total amount of change to be returned to the customer.
        // This is the sum of all denominations multiplied by their respective counts.
        public decimal TotalChange { get; set; }
    }
}