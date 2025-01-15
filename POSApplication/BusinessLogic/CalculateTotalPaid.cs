namespace POSApplication.BusinessLogic.Utilities.Payments;
public class Calculate
{
    public decimal TotalPaid(Dictionary<decimal, int> paymentInDenominations)
    {
        return paymentInDenominations.Sum(entry => entry.Key * entry.Value);
    }
}