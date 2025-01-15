using System.Text.Json;
using POSApplication.Data;
using POSApplication.Data.Models;

public class CurrencyConfigDemo
{
    // Singleton instance of ManualCurrencyConfig
    private static readonly Lazy<CurrencyConfigDemo> _instance = new(() => new CurrencyConfigDemo());
    public static CurrencyConfigDemo Instance => _instance.Value;

    // Holds the currency data for all countries
    private Dictionary<string, List<decimal>> _currencies;

    // Provides read-only access to the currencies
    public IReadOnlyDictionary<string, IReadOnlyList<decimal>> GetCurrencies() =>
        _currencies.ToDictionary(
            kvp => kvp.Key,
            kvp => (IReadOnlyList<decimal>) kvp.Value.AsReadOnly()
        );

    public CurrencyConfigDemo()
    {
        _currencies = new Dictionary<string, List<decimal>>();
        // Load configuration from the JSON file
        LoadFromFile("ManualCurrencyConfig.json");
    }

    public void LoadFromFile(string filename)
    {
        try
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The configuration file {filename} could not be found in {filePath}.");
            }

            var json = File.ReadAllText(filePath);
            var config = JsonSerializer.Deserialize<CurrencyFile>(json);

            if (config?.Currencies == null || config.Currencies.Count == 0)
            {
                throw new InvalidDataException($"Invalid or empty configuration file {filename}.");
            }

            foreach (var currency in config.Currencies)
            {
                if (currency.Denominations == null || currency.Denominations.Count == 0)
                {
                    throw new InvalidDataException($"Currency {currency.CurrencyCode} has no denominations.");
                }

                // Ensure denominations are sorted in descending order
                currency.Denominations.Sort((a, b) => b.CompareTo(a));
                _currencies[currency.CurrencyCode] = new List<decimal>(currency.Denominations);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading the configuration: {ex.Message}");
        }
    }

    // Retrieves denominations for a specific currency code
    public IReadOnlyList<decimal> GetDenominationsByCurrencyCode(string currencyCode)
    {
        if (_currencies.TryGetValue(currencyCode, out var denominations))
        {
            return denominations.AsReadOnly();
        }

        throw new KeyNotFoundException($"Currency with code {currencyCode} not found.");
    }

    // Sets currencies with validation
    public void SetCurrencies(Dictionary<string, List<decimal>> currencies)
    {
        if (currencies == null || currencies.Count == 0)
        {
            throw new ArgumentException("The currencies dictionary cannot be null or empty.");
        }

        var validatedCurrencies = new Dictionary<string, List<decimal>>();

        foreach (var entry in currencies)
        {
            if (string.IsNullOrWhiteSpace(entry.Key))
            {
                throw new ArgumentException("Currency code must not be null, empty, or whitespace.");
            }

            if (entry.Value == null || entry.Value.Count == 0)
            {
                throw new ArgumentException($"Denominations for currency {entry.Key} must not be null or empty.");
            }

            // Sort denominations in descending order
            var sortedDenominations = new List<decimal>(entry.Value);
            sortedDenominations.Sort((a, b) => b.CompareTo(a));
            validatedCurrencies[entry.Key] = sortedDenominations;
        }

        // Replace the current currencies with the new validated set
        _currencies = validatedCurrencies;
    }
}