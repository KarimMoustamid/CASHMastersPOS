namespace CASHMastersPOS.Tests
{
    public class CurrencyConfigTests
    {
        // Mock file content for successful test cases
        private const string ValidJsonFile = @"{
        ""Currencies"": [
            {
                ""Country"": ""US"",
                ""CurrencyCode"":""USD"",
                ""Denominations"": [100, 50, 20, 10, 5, 1, 0.25, 0.1, 0.05, 0.01]
            },
            {
                ""Country"": ""Mexico"",
                ""CurrencyCode"":""MXN"",
                ""Denominations"": [1000, 500, 200, 100, 50, 20, 10, 5, 2, 1, 0.5]
            }
            ]
        }";

        [Fact]
        public void Instance_Should_ReturnSameInstance()
        {
            // Act
            var instance1 = CurrencyConfig.Instance;
            var instance2 = CurrencyConfig.Instance;

            // Assert
            Assert.Equal(instance1, instance2);
        }
    }
}