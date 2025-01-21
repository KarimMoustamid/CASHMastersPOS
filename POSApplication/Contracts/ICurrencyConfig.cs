namespace POSApplication.Data
{
    using Models;

    // Interface defining the contract for handling currency configurations.
    // Enables managing currencies, including initialization, selection, and retrieval of denominations and details.
    public interface ICurrencyConfig
    {
        // Initializes the currency configuration with a list of pre-configured currencies.
        // Parameters:
        // - preConfiguredCurrencies: A list of CurrencyData objects used to set up the initial currency configuration.
        void Initialize(List<CurrencyData> preConfiguredCurrencies);

        // Sets the active currency using its unique currency code.
        // Parameters:
        // - currencyCode: The code of the currency to be set (e.g., "USD", "EUR").
        void SetCurrency(string currencyCode);

        // Retrieves details of the currently active currency.
        // Returns:
        // - A CurrencyData object representing the current currency or null if no currency is selected.
        CurrencyData? GetCurrency();

        // Retrieves a read-only list of all available currencies loaded from the configuration.
        // Returns:
        // - An IReadOnlyList of CurrencyData objects representing all available currencies.
        IReadOnlyList<CurrencyData?> GetAvailableCurrencies();

        // Retrieves a read-only list of the denominations for the currently selected currency.
        // Returns:
        // - An IReadOnlyList of decimal values representing the denominations.
        IReadOnlyList<decimal> GetDenominations();
    }
}