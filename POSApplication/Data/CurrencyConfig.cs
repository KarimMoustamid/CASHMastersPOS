namespace POSApplication.Data
{
    using System.Runtime.InteropServices.ComTypes;
    using System.Text.Json;
    using BusinessLogic.Utilities;
    using Models;

    public class CurrencyConfig
    {
        // Singleton instance of CurrencyConfig to ensure only one instance is created
        // Lazy<T> : provide lazy initialization (object is created only when it´s accessed for the first time) , Lazy<T> is thread safe .
        // _instance : it will be only accessed within the class , it´s shared across all instances of the class and cannot be reassigned
        private static readonly Lazy<CurrencyConfig> _instance = new(() => new CurrencyConfig());
        public static CurrencyConfig Instance => _instance.Value; // accessing the .Value will execute the initialization logic

        // represent currency denominations
        private List<decimal> _denominations;


        //provides a snapshot of _currencies's current state in a read-only form.
        // Any attempt to modify it will throw a NotSupportedException. internal state will be encapsulated and protected .
        public IReadOnlyList<decimal> GetDenominations() => _denominations.AsReadOnly();

        public void SetDenominations(List<decimal> denominations)
        {
            if (denominations == null || denominations.Count == 0)
            {
                throw new ArgumentException("Denominations cannot be null or empty.");
            }

            denominations.Sort((a, b) => b.CompareTo(a)); // Descending order
            _denominations = new List<decimal>(denominations);
        }

        public CurrencyConfig()
        {
            _denominations = new List<decimal>();
            // Load configuration from the JSON file
            LoadFromFile("CurrencyConfig.json");
        }


        // TODO : Create a LoadFromFile Method
        public void LoadFromFile(string filename)
        {
            try
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

                // Testing Load us denominations
                var usCurrency = config.Currencies.FirstOrDefault(c => c.CurrencyCode == CurrencyConstants.USD);
                if (usCurrency == null || usCurrency.Denominations == null || usCurrency?.Denominations?.Count == 0)
                {
                    throw new InvalidDataException($"US denominations not found in file {filename}.");
                }

                SetDenominations(usCurrency?.Denominations!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the configuration: {ex.Message}");
            }
        }
    }
}