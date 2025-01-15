    using System.Text.Json;
    using POSApplication.Data.Models;

    public class CurrencyConfig
    {
        // Singleton instance of ManualCurrencyConfig to ensure only one instance is created
        // Lazy<T> : provide lazy initialization (object is created only when it´s accessed for the first time) , Lazy<T> is thread safe .
        // _instance : it will be only accessed within the class , it´s shared across all instances of the class and cannot be reassigned
        private static readonly Lazy<CurrencyConfig> _instance = new(() => new CurrencyConfig());
        public static CurrencyConfig Instance => _instance.Value; // accessing the .Value will execute the initialization logic

        // represent currency denominations
        private List<decimal>? _denominations = new();

        private string? _currencyCountry = string.Empty;

        private CurrencyData _currentCurrency = new();

        public CurrencyConfig()
        {
            // Load all currencies from the JSON file during initialization
            LoadFromFile("CurrencyConfig.json");
        }

        private List<CurrencyData> _currencies = new();

        public void LoadFromFile(string filename)
        {
            // Building the file path relative to the application's base directory
            // string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            // Console.WriteLine($"Loading configuration file from : {filePath}");

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The configuration file {filename} could not be found in {filePath}.");
            }

            // Read the file content
            var json = File.ReadAllText(filePath);
            // Deserialize the JSON content into the currency configuration model
            var config = JsonSerializer.Deserialize<CurrencyFile>(json);

            // Validate  and assign the  loaded data
            if (config?.Currencies == null || config.Currencies.Count == 0)
            {
                throw new InvalidDataException($"Invalid or empty configuration file {filename}.");
            }

            _currencies = config.Currencies;
        }

        // TODO : Separation of Concern
        public void SetCurrency(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                throw new ArgumentNullException(nameof(currencyCode), "Currency code cannot be null, empty, or consist only of white-space characters.");
            }

            // Search for the currency
            var currency = _currencies.FirstOrDefault(c => string.Equals(c.CurrencyCode, currencyCode, StringComparison.OrdinalIgnoreCase));

            if (currency == null)
            {
                throw new KeyNotFoundException(
                    $"The currency code '{currencyCode}' was not found. Please ensure that the code is valid and exists in the available currencies.");
            }

            if (currency.Denominations == null || currency.Denominations.Count == 0)
            {
                throw new InvalidOperationException($"The currency code '{currency.CurrencyCode}' does not have any valid denominations.");
            }

            // Sort denominations in descending order
            currency.Denominations.Sort((a, b) => b.CompareTo(a));

            _denominations = new List<decimal>(currency.Denominations);
            _currencyCountry = currency.CurrencyCode;
            _currentCurrency = currency;
        }

        public CurrencyData? GetCurrency() => _currentCurrency;

        public IReadOnlyList<CurrencyData?> GetAvailableCurrencies() => _currencies?.AsReadOnly()!;

        public IReadOnlyList<decimal> GetDenominations()
        {
            if (string.IsNullOrWhiteSpace(_currencyCountry))
            {
                throw new InvalidOperationException("No currency has been selected ");
            }
            return _denominations?.AsReadOnly();
        }
    }