using POSApplication.Data;
Console.WriteLine("Welcome to the CASH Masters POS System!\n");

var denominations = CurrencyConfig.Instance.GetDenominations();
Console.WriteLine("Loaded Denominations:\n");

foreach (var denomination in denominations)
{
    Console.WriteLine(denomination);
}