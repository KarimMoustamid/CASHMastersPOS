namespace POSApplication.Data
{
    using Models;

    public interface ICurrencyConfig
    {
        void Initialize(string configFilePath);
        void LoadFromFile(string filename);
        void SetCurrency(string currencyCode);
        CurrencyData? GetCurrency();
        IReadOnlyList<CurrencyData?> GetAvailableCurrencies();
        IReadOnlyList<decimal> GetDenominations();
    }
}