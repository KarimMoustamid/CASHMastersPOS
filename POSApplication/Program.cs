Console.WriteLine("Welcome to the CASH Masters POS System!\n");

// var denominations = CurrencyConfig.Instance.GetDenominations();
// Console.WriteLine("Loaded Denominations:\n");
//
// foreach (var denomination in denominations)
// {
//     Console.WriteLine(denomination);
// }
//

// Demo :

// Access the singleton instance
var config = CurrencyConfigDemo.Instance;

// Display existing currencies
var currencies = config.GetCurrencies();
Console.WriteLine("Existing Currencies:");
foreach (var currency in currencies)
{
    Console.WriteLine($"Currency Code: {currency.Key}, Denominations: {string.Join(", ", currency.Value)}");
}