namespace POSApplication.BusinessLogic.config
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigLoader
    {
        private static IConfiguration? _configuration;

        // Static method to initialize configuration
        public static IConfiguration LoadConfiguration()
        {
            if (_configuration == null)
            {
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            }

            return _configuration;
        }

        // Optional: Use helper methods to get specific configuration values easily
        public static string GetCurrencyFilePath()
        {
            var configuration = LoadConfiguration();
            var filePath = configuration["CurrencyFilePath"];
            if (string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("CurrencyFilePath is missing or empty in appsettings.json.");
            }

            return filePath;
        }
    }
}