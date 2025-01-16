namespace POSApplication.Presentation.Utilities.logs
{
    public interface IUserInteractionHelper
    {
        void CurrencyDenominations();
        void DisplayAvailableCurrencies();
        T GetInput<T>(string prompt, string errorMessage);
        Dictionary<decimal, int> CollectPaymentInput();
    }
}