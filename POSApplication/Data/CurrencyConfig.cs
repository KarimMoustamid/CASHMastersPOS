namespace POSApplication.Data
{
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Models;

    public class CurrencyConfig : ICurrencyConfig
    {
        private readonly ILogger<CurrencyConfig> _logger;
        private List<decimal>? _denominations = new();
        private string? _currencyCountry = string.Empty;
        private CurrencyData _currentCurrency = new();
        private List<CurrencyData> _currencies = new();

        public CurrencyConfig(ILogger<CurrencyConfig> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Initialize(string configFilePath)
        {
            try
            {
                LoadFromFile(configFilePath);
                _logger.LogInformation("Currency configuration loaded successfully from {FilePath}.", configFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize currency configuration from {FilePath}.", configFilePath);
                throw;
            }
        }

        public void LoadFromFile(string filename)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            _logger.LogDebug("Looking for configuration file at: {FilePath}", filePath);


            if (!File.Exists(filePath))
            {
                _logger.LogError("Configuration file {FileName} could not be found at {FilePath}.", filename, filePath);
                throw new FileNotFoundException($"The configuration file {filename} could not be found in {filePath}.");
            }

            try
            {
                var json = File.ReadAllText(filePath);
                var config = JsonSerializer.Deserialize<CurrencyFile>(json);

                if (config?.Currencies == null || config.Currencies.Count == 0)
                {
                    _logger.LogError("Invalid or empty configuration file {FileName}.", filename);
                    throw new InvalidDataException($"Invalid or empty configuration file {filename}.");
                }

                _currencies = config.Currencies;
                _logger.LogInformation("Loaded {Count} currencies from {FileName}.", _currencies.Count, filename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading configuration file {FileName}.", filename);
                throw;
            }
        }

        public void SetCurrency(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                _logger.LogError("Currency code cannot be null, empty, or whitespace.");
                throw new ArgumentNullException(nameof(currencyCode), "Currency code cannot be null, empty, or consist only of white-space characters.");
            }

            var currency = _currencies.FirstOrDefault(c => string.Equals(c.CurrencyCode, currencyCode, StringComparison.OrdinalIgnoreCase));

            if (currency == null)
            {
                _logger.LogError("Currency code '{CurrencyCode}' not found.", currencyCode);
                throw new KeyNotFoundException($"The currency code '{currencyCode}' was not found.");
            }

            if (currency.Denominations == null || currency.Denominations.Count == 0)
            {
                _logger.LogError("Currency code '{CurrencyCode}' does not have valid denominations.", currency.CurrencyCode);
                throw new InvalidOperationException($"The currency code '{currency.CurrencyCode}' does not have any valid denominations.");
            }

            currency.Denominations.Sort((a, b) => b.CompareTo(a));

            _denominations = new List<decimal>(currency.Denominations);
            _currencyCountry = currency.CurrencyCode;
            _currentCurrency = currency;

            _logger.LogInformation("Currency set to {CurrencyCode} with {DenominationCount} denominations.",
                _currencyCountry,
                _denominations.Count);
        }

        public CurrencyData? GetCurrency() => _currentCurrency;

        public IReadOnlyList<CurrencyData?> GetAvailableCurrencies() => _currencies?.AsReadOnly()!;

        public IReadOnlyList<decimal> GetDenominations()
        {
            if (string.IsNullOrWhiteSpace(_currencyCountry))
            {
                _logger.LogError("No currency has been selected.");
                throw new InvalidOperationException("No currency has been selected.");
            }

            return _denominations?.AsReadOnly();
        }
    }
}