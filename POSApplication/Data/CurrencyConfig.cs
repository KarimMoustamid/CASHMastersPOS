using Microsoft.Extensions.Logging;
using POSApplication.Data;
using POSApplication.Data.Models;

public class CurrencyConfig : ICurrencyConfig
{
    private readonly ILogger<CurrencyConfig> _logger;
    private List<decimal> _denominations = new();
    private string _currencyCountry = string.Empty;
    private CurrencyData? _currentCurrency;
    private List<CurrencyData> _currencies = new();

    public CurrencyConfig(ILogger<CurrencyConfig> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Initialize(List<CurrencyData> preConfiguredCurrencies)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(preConfiguredCurrencies, nameof(preConfiguredCurrencies));

            if (preConfiguredCurrencies.Count == 0)
            {
                _logger.LogError("Pre-configured currencies are empty.");
                throw new ArgumentException("Pre-configured currencies cannot be empty.", nameof(preConfiguredCurrencies));
            }

            _currencies = preConfiguredCurrencies.ToList(); // Clone to ensure immutability
            _logger.LogInformation("Loaded {Count} currencies from configuration.", _currencies.Count);
        }
        catch (Exception ex)
        {
            _currencies.Clear();
            _logger.LogError(ex, "Error while loading pre-configured currencies.");
            throw;
        }
    }

    public void SetCurrency(string currencyCode)
    {
        ArgumentNullException.ThrowIfNull(currencyCode, nameof(currencyCode));

        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            _logger.LogError("Currency code cannot be null or empty.");
            throw new ArgumentException("Currency code cannot be empty or consist only of white-space characters.", nameof(currencyCode));
        }

        var currency = _currencies.FirstOrDefault(c => string.Equals(c.CurrencyCode, currencyCode, StringComparison.OrdinalIgnoreCase));
        if (currency == null)
        {
            _logger.LogError("Currency code '{currencyCode}' not found.", currencyCode);
            throw new KeyNotFoundException($"Currency code '{currencyCode}' was not found.");
        }

        if (currency.Denominations == null || currency.Denominations.Count == 0)
        {
            _logger.LogError("Currency code '{CurrencyCode}' does not have valid denominations.", currency.CurrencyCode);
            throw new InvalidOperationException($"Currency code '{currency.CurrencyCode}' does not have any valid denominations.");
        }

        _denominations = new List<decimal>(currency.Denominations.OrderByDescending(d => d));
        _currentCurrency = currency;
        _currencyCountry = currency.CurrencyCode;
        _logger.LogInformation("Currency set to {CurrencyCode} with {DenominationCount} denominations.",
            _currencyCountry,
            _denominations.Count);
    }

    public CurrencyData? GetCurrency() => _currentCurrency;

    public IReadOnlyList<CurrencyData> GetAvailableCurrencies() => _currencies.AsReadOnly();

    public IReadOnlyList<decimal> GetDenominations()
    {
        if (string.IsNullOrWhiteSpace(_currencyCountry))
        {
            _logger.LogError("No currency has been selected.");
            throw new InvalidOperationException("No currency has been selected.");
        }

        // Return immutable copy
        return new List<decimal>(_denominations).AsReadOnly();
    }
}