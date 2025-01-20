namespace POSApplication.Data
{
    // The `System.Text.Json` namespace is used for handling JSON serialization and deserialization.
    using System.Text.Json;
    // The `Microsoft.Extensions.Logging` namespace is used for logging functionality in the application.
    using Microsoft.Extensions.Logging;
    // Imports the models namespace where `CurrencyData` and `CurrencyFile` are defined.
    using Models;

    public class CurrencyConfig : ICurrencyConfig
    {
        // Logger instance for logging messages and errors related to currency configuration.
        private readonly ILogger<CurrencyConfig> _logger;

        // List of denominations (e.g., [100, 50, 20, 10]) for the currently selected currency.
        private List<decimal>? _denominations = new();

        // Currently selected currency code (e.g., "USD", "MXN").
        private string? _currencyCountry = string.Empty;

        // Holds the details of the currently selected currency.
        private CurrencyData _currentCurrency = new();

        // List of all available currencies loaded from the configuration file.
        private List<CurrencyData> _currencies = new();

        // Constructor for initializing the class with a logger instance.
        // Throws an exception if the logger is not provided (null).
        public CurrencyConfig(ILogger<CurrencyConfig> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Initializes the currency configuration by loading data from a given configuration file path.
        // Logs success or error messages based on the initialization outcome.
        public void Initialize(string configFilePath)
        {
            try
            {
                // Loads the configuration file.
                LoadFromFile(configFilePath);

                _logger.LogInformation("Currency configuration loaded successfully from {FilePath}.", configFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize currency configuration from {FilePath}.", configFilePath);
                throw;
            }
        }

        // Loads currency data from a specified JSON configuration file.
        // Validates the file's existence and its validity before loading data into the `_currencies` field.
        public void LoadFromFile(string filename)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            _logger.LogDebug("Looking for configuration file at: {FilePath}", filePath);

            // Checks if the file exists; throws an exception if it doesn't.
            if (!File.Exists(filePath))
            {
                _logger.LogError("Configuration file {FileName} could not be found at {FilePath}.", filename, filePath);
                throw new FileNotFoundException($"The configuration file {filename} could not be found in {filePath}.");
            }

            try
            {
                // Reads and deserializes the JSON file into a `CurrencyFile` object.
                var json = File.ReadAllText(filePath);
                var config = JsonSerializer.Deserialize<CurrencyFile>(json);

                // Validates the file's content.
                if (config?.Currencies == null || config.Currencies.Count == 0)
                {
                    _logger.LogError("Invalid or empty configuration file {FileName}.", filename);
                    throw new InvalidDataException($"Invalid or empty configuration file {filename}.");
                }

                // Stores the loaded currencies in the `_currencies` field.
                _currencies = config.Currencies;

                _logger.LogInformation("Loaded {Count} currencies from {FileName}.", _currencies.Count, filename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading configuration file {FileName}.", filename);
                throw;
            }
        }

        // Sets the currently active currency based on the provided currency code.
        // Sorts its denominations in descending order and updates the local state for denominational calculations.
        public void SetCurrency(string currencyCode)
        {
            // Validates the currency code parameter.
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                _logger.LogError("Currency code cannot be null, empty, or whitespace.");
                throw new ArgumentNullException(nameof(currencyCode), "Currency code cannot be null, empty, or consist only of white-space characters.");
            }

            // Finds the currency matching the provided code.
            var currency = _currencies.FirstOrDefault(c => string.Equals(c.CurrencyCode, currencyCode, StringComparison.OrdinalIgnoreCase));

            // Ensures the currency exists.
            if (currency == null)
            {
                _logger.LogError("Currency code '{CurrencyCode}' not found.", currencyCode);
                throw new KeyNotFoundException($"The currency code '{currencyCode}' was not found.");
            }

            // Ensures that the currency has valid denominations.
            if (currency.Denominations == null || currency.Denominations.Count == 0)
            {
                _logger.LogError("Currency code '{CurrencyCode}' does not have valid denominations.", currency.CurrencyCode);
                throw new InvalidOperationException($"The currency code '{currency.CurrencyCode}' does not have any valid denominations.");
            }

            // Sorts denominations in descending order (largest to smallest for easier calculations).
            currency.Denominations.Sort((a, b) => b.CompareTo(a));

            // Updates the internal state with the selected currency details.
            _denominations = new List<decimal>(currency.Denominations);
            _currencyCountry = currency.CurrencyCode;
            _currentCurrency = currency;

            _logger.LogInformation("Currency set to {CurrencyCode} with {DenominationCount} denominations.",
                _currencyCountry,
                _denominations.Count);
        }

        // Retrieves the currently selected currency details.
        public CurrencyData? GetCurrency() => _currentCurrency;

        // Retrieves the list of all available currencies as a read-only collection.
        public IReadOnlyList<CurrencyData?> GetAvailableCurrencies() => _currencies?.AsReadOnly()!;

        // Retrieves the denominations for the currently selected currency.
        public IReadOnlyList<decimal> GetDenominations()
        {
            // Ensures that a currency has been selected before retrieving denominations.
            if (string.IsNullOrWhiteSpace(_currencyCountry))
            {
                _logger.LogError("No currency has been selected.");
                throw new InvalidOperationException("No currency has been selected.");
            }

            return _denominations?.AsReadOnly();
        }
    }
}