namespace POSApplication.BusinessLogic.config
{
    // Including the namespace for Microsoft.Extensions.Configuration, which provides abstractions and various implementations
    // for configuring .NET applications. This is commonly used for handling configuration from sources like appsettings.json.
    using Microsoft.Extensions.Configuration;

    public static class ConfigLoader
    {
        // Static field to hold the loaded configuration instance.
        // This ensures the configuration is loaded only once and reused when needed.
        private static IConfiguration? _configuration;

        // Static method to initialize and load the configuration object.
        // This uses a ConfigurationBuilder to read configuration settings
        // from the project's "appsettings.json" file.
        public static IConfiguration LoadConfiguration()
        {
            // Check if the configuration has already been initialized.
            if (_configuration == null)
            {
                // Initialize the ConfigurationBuilder.
                // - Sets the base path to the current application's base directory.
                // - Loads "appsettings.json", making it a required file (optional: false).
                // - Allows watching for changes in the file and reloads the configuration accordingly.
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            }

            // Return the initialized configuration object.
            return _configuration;
        }

        // Helper method to retrieve the "CurrencyFilePath" setting from the configuration.
        // This makes it easier to access specific configuration values without exposing
        // the entire IConfiguration object.
        public static string GetCurrencyFilePath()
        {
            // Get the "CurrencyFilePath" value from the configuration.
            var configuration = LoadConfiguration();

            // Validate that the retrieved value is not null or empty.
            // If the value is missing or empty, throw an exception to indicate the issue.
            var filePath = configuration["CurrencyFilePath"];
            if (string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("CurrencyFilePath is missing or empty in appsettings.json.");
            }

            // Return the valid file path.
            return filePath;
        }
    }
}