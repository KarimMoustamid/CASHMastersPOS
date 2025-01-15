namespace POSApplication.Data
{
    public class CurrencyConfig
    {
        // Singleton instance of CurrencyConfig to ensure only one instance is created
        // Lazy<T> : provide lazy initialization (object is created only when it´s accessed for the first time) , Lazy<T> is thread safe .
        // _instance : it will be only accessed within the class , it´s shared across all instances of the class and cannot be reassigned
        private static readonly Lazy<CurrencyConfig> _instance = new(() => new CurrencyConfig());
        public static CurrencyConfig Instance => _instance.Value; // accessing the .Value will execute the initialization logic

        // represent currency denominations
        private List<decimal> _denominations;
        public CurrencyConfig()
        {
            _denominations = new List<decimal>();
            //  TODO : Load the configuration
        }


        // TODO : Create a LoadFromFile Method
    }
}