namespace POSApplication.Tests
{
    using Data;
    using Data.Models;
    using Moq;

    public class CurrencyConfigTests
    {
        [Fact]
        public void SetCurrency_ValidCurrency_SetsCurrencySuccessfully()
        {
            // Arrange
            var mockCurrencyConfig = new Mock<ICurrencyConfig>();
            var currencyData = new CurrencyData
            {
                CurrencyCode = "USD",
                Denominations = new List<decimal> { 100, 50, 20, 10, 5, 1, 0.25m }
            };

            mockCurrencyConfig.Setup(c => c.GetCurrency()).Returns(currencyData);

            // Act
            var selectedCurrency = mockCurrencyConfig.Object.GetCurrency();

            // Assert
            Assert.NotNull(selectedCurrency);
            Assert.Equal("USD", selectedCurrency?.CurrencyCode);
        }
    }
}