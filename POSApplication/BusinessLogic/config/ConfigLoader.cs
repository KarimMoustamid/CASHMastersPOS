using Microsoft.Extensions.Configuration;
using POSApplication.Data.Models;

public static class ConfigLoader
{
    private static IConfiguration? _configuration;
    private static readonly object _lock = new object(); // Ensure thread safety
    private const string ConfigFileName = "CurrencyConfig.json";

    // Lazily load the configuration with thread safety
    public static IConfiguration LoadConfiguration()
    {
        if (_configuration == null)
        {
            lock (_lock) // Use a lock to ensure thread safety
            {
                if (_configuration == null)
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile(ConfigFileName, optional: false, reloadOnChange: true)
                        .Build();
                }
            }
        }

        return _configuration;
    }

    // Retrieve all currencies from the configuration
    public static List<CurrencyData> LoadAllCurrenciesFromConfig()
    {
        try
        {
            var configuration = LoadConfiguration();
            var currencies = configuration.GetSection("Currencies").Get<List<CurrencyData>>();
            if (currencies == null || currencies.Count == 0)
            {
                throw new InvalidOperationException("No currencies found in the configuration file.");
            }

            return currencies;
        }
        catch (Exception ex)
        {
            // Log the error appropriately (use a structured logging library in production)
            Console.Error.WriteLine($"Error loading currencies from configuration: {ex}");

            // Optionally rethrow or indicate an issue
            throw new InvalidOperationException("Failed to load currencies from configuration.", ex);
        }
    }

    public static string GetConfigurationFilePath()
    {
        // Combines the application's base directory with the filename to get the full path of the configuration file
        return Path.Combine(AppContext.BaseDirectory, ConfigFileName);
    }


}