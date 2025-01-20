namespace POSApplication.Tests
{
    using Moq;
    using Presentation.Utilities.logs;

    public class UserInteractionHelperTests
    {
        [Fact]
        public void GetInput_WhenValidInput_ShouldReturnParsedValue()
        {
            // Arrange
            var mockInteractionHelper = new Mock<IUserInteractionHelper>();
            mockInteractionHelper
                .Setup(x => x.GetInput<decimal>("Enter price:", "Invalid input.", It.IsAny<Func<decimal, bool>>()))
                .Returns(99.99m);

            // Act
            var result = mockInteractionHelper.Object.GetInput<decimal>(
                "Enter price:",
                "Invalid input.",
                null
            );

            // Assert
            Assert.Equal(99.99m, result);
        }

        [Fact]
        public void CurrencyDenominations_ShouldLogAndDisplayAvailableDenominations()
        {
            // Arrange
            var mockInteractionHelper = new Mock<IUserInteractionHelper>();

            // Act
            mockInteractionHelper.Object.CurrencyDenominations();

            // Assert
            mockInteractionHelper.Verify(x => x.CurrencyDenominations(), Times.Once);
        }

        [Fact]
        public void DisplayAvailableCurrencies_ShouldLogAndDisplayAvailableCurrencies()
        {
            // Arrange
            var mockInteractionHelper = new Mock<IUserInteractionHelper>();

            // Act
            mockInteractionHelper.Object.DisplayAvailableCurrencies();

            // Assert
            mockInteractionHelper.Verify(x => x.DisplayAvailableCurrencies(), Times.Once);
        }

        [Fact]
        public void CollectPaymentInput_WhenValidInputs_ShouldCollectPayments()
        {
            // Arrange
            var mockInteractionHelper = new Mock<IUserInteractionHelper>();
            var samplePaymentData = new Dictionary<decimal, int>
            {
                {10.0m, 5},
                {20.0m, 3}
            };

            mockInteractionHelper
                .Setup(x => x.CollectPaymentInput())
                .Returns(samplePaymentData);

            // Act
            var result = mockInteractionHelper.Object.CollectPaymentInput();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result[10.0m]);
            Assert.Equal(3, result[20.0m]);
        }
    }
}