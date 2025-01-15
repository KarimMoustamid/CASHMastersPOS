using POSApplication.Data.Models;

public static class InputValidator
{
    public static void ValidatePayment(Payment payment, List<decimal> validDenominations)
    {
        foreach (var denom in payment.Denominations.Keys)
        {
            if (!validDenominations.Contains(denom))
            {
                throw new ArgumentException($"Invalid denomination: {denom}");
            }
        }

        if (payment.TotalPaid < 0)
        {
            throw new ArgumentException("Total paid cannot be negative.");
        }
    }

    public static void ValidatePrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative.");
        }
    }
}