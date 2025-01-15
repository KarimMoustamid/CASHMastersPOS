using POSApplication.Data.AfterRefinmentDemo;

Console.WriteLine("Welcome to the CASH Masters POS System!\n");

// Display available countries
var availableCountries = CurrencyConfig.Instance.GetAvailableCountries();
Console.WriteLine("Available Currencies :\n");
foreach (var country in availableCountries)
{
   Console.WriteLine($"- {country}");
}