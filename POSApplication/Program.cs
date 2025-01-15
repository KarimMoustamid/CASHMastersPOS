using POSApplication.Data.AfterRefinmentDemo;
using POSApplication.Data.Models;


try
{
    Console.WriteLine("\nWelcome to the CASH Masters POS System!\n");

// Display available countries
    var availableCurrencies = CurrencyConfig.Instance.GetAvailableCurrencies();

    Console.WriteLine("\nAvailable Currencies :\n");
    foreach (var currency in availableCurrencies)
    {
        Console.WriteLine($"- {currency.Country} ({currency.CurrencyCode})\n");
    }


// Let the user choose a country
    Console.Write("\nEnter the currency Code for the currency configuration: ");
    var currencyCode = Console.ReadLine();

// Set the selected currency
//TODO : Handle input validation
    CurrencyConfig.Instance.SetCurrency(currencyCode);

    Console.WriteLine("\nCurrency loaded successfully!: ");
    var denominations = CurrencyConfig.Instance.GetDenominations();
    Console.WriteLine("\nDenominations :\n");
    foreach (var denom in denominations)
    {
        Console.WriteLine($"- {denom:C}\n");
    }

// change calculation
    Console.Write("\nEnter the price of the item(s): ");
    var price = decimal.Parse(Console.ReadLine() ?? "0");

    Console.Write("Enter the amount paid by the customer: ");
    var paid = decimal.Parse(Console.ReadLine() ?? "0");

// var change = Implement CalculateChange(price, paid);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}