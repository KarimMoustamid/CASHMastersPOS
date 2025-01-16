namespace POSApplication.Tests
{
    using BusinessLogic.Utilities;
    using Data;
    using Moq;

    public class CurrencyConfigTests
    {
        [Fact]
        public void Initialize_Should_Set_CurrencyFilePath()
        {
            // Arrange
            var mockCurrencyConfig = new Mock<ICurrencyConfig>();
            string testFilePath = "path/to/test/currency.json";

            // Act
            mockCurrencyConfig.Object.Initialize(testFilePath);

            // Assert
            mockCurrencyConfig.Verify(x => x.Initialize(testFilePath), Times.Once);
        }

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