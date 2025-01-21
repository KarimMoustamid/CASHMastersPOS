namespace POSApplication.Tests
{
    using BusinessLogic.Utilities;
    using Data;
    using Moq;

    public class CurrencyConfigTests
    {
        [Fact]
        public void SetCurrency_Should_Set_Default_Currency()
        {
            // Arrange
            var mockCurrencyConfig = new Mock<ICurrencyConfig>();

            // Act
            mockCurrencyConfig.Object.SetCurrency(CurrencyConstants.USD);

            // Assert
            mockCurrencyConfig.Verify(x => x.SetCurrency(CurrencyConstants.USD), Times.Once);
        }



    }
}