namespace POSApplication.BusinessLogic
{
    using Data.Models;

    public interface IChangeCalculator
    {
        Change CalculateChange(decimal price, Payment payment, string currencyCode);
    }
}