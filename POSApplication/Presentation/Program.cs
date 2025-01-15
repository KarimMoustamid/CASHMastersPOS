using POSApplication.BusinessLogic;
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
    var currencyCode = Console.ReadLine()?.ToUpper();

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
    var price = decimal.Parse(Console.ReadLine() ?? "0"); //  if no input is provided , the fallback value is "0" .

    Console.Write("Register the payment breakdown by denomination and coins: \n");
    var payment = new Dictionary<decimal, int>();

    while (true)
    {
        Console.Write("Denomination :");
        var denom = decimal.Parse(Console.ReadLine() ?? "0");

        Console.Write("Count: ");
        var count = int.Parse(Console.ReadLine() ?? "0");

        if (payment.ContainsKey(denom))
            payment[denom] += count;
        else
            payment[denom] = count;

        Console.Write("Add another denomination? (y/n): ");
        if (Console.ReadLine()?.ToLower() != "y")
            break;
    }

    // Calculate Change
    var change = ChangeCalculator.CalculateChange(price, payment, currencyCode);

    Console.WriteLine("\nChange to return:");
    foreach (var item in change)
    {
        Console.WriteLine($"{item.Value} x {item.Key:C}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}