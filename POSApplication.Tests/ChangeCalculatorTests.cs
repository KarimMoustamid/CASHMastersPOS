using System;
using System.Collections.Generic;
using Moq;
using POSApplication.BusinessLogic;
using POSApplication.Data;
using POSApplication.Data.Models;
using Xunit;

namespace POSApplication.Tests
{
    public class ChangeCalculatorTests
    {
        private readonly Mock<ICurrencyConfig> _mockCurrencyConfig;

        public ChangeCalculatorTests()
        {
            _mockCurrencyConfig = new Mock<ICurrencyConfig>();

            // Setup default currency configuration used in tests
            _mockCurrencyConfig.Setup(config => config.GetCurrency())
                .Returns(new CurrencyData
                {
                    CurrencyCode = "USD",
                    Denominations = new List<decimal>
                    {
                        0.01m, 0.05m, 0.10m, 0.25m, 1.00m, 5.00m, 10.00m, 20.00m
                    }
                });

            _mockCurrencyConfig.Setup(config => config.GetDenominations())
                .Returns(new List<decimal>
                {
                    0.01m, 0.05m, 0.10m, 0.25m, 1.00m, 5.00m, 10.00m, 20.00m
                });
        }

        [Fact]
        public void CalculateChange_WithValidInputs_ShouldReturnCorrectChange()
        {
            // Arrange
            var calculator = new ChangeCalculator(_mockCurrencyConfig.Object);
            var price = 10.50m;
            var payment = new Payment
            {
                TotalPaid = 20.00m,
                Denominations = new Dictionary<decimal, int>
                {
                    {10.00m, 2}
                }
            };

            // Act
            var change = calculator.CalculateChange(price, payment, "USD");

            // Assert
            Assert.NotNull(change);
            Assert.Equal(9.50m, change.TotalChange);
            Assert.Equal(1, change.Denominations[5.00m]); // 1 five-dollar bill
            Assert.Equal(2, change.Denominations[0.25m]); // 2 quarters
        }

        [Fact]
        public void CalculateChange_WithExactPayment_ShouldReturnZeroChange()
        {
            // Arrange
            var calculator = new ChangeCalculator(_mockCurrencyConfig.Object);
            var price = 15.75m;
            var payment = new Payment
            {
                TotalPaid = 15.75m,
                Denominations = new Dictionary<decimal, int>
                {
                    {10.00m, 1},
                    {5.00m, 1},
                    {0.25m, 3}
                }
            };

            // Act
            var change = calculator.CalculateChange(price, payment, "USD");

            // Assert
            Assert.NotNull(change);
            Assert.Equal(0.00m, change.TotalChange);
            Assert.Empty(change.Denominations); // No change to return
        }

        [Fact]
        public void CalculateChange_WithInvalidCurrencyCode_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var calculator = new ChangeCalculator(_mockCurrencyConfig.Object);
            var price = 10.00m;
            var payment = new Payment
            {
                TotalPaid = 20.00m,
                Denominations = new Dictionary<decimal, int>
                {
                    {20.00m, 1}
                }
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => calculator.CalculateChange(price, payment, "EUR"));
            Assert.Contains("The currency code 'EUR' is not currently loaded.", exception.Message);
        }

        [Fact]
        public void CalculateChange_WithInvalidDenominations_ShouldThrowArgumentException()
        {
            // Arrange
            var calculator = new ChangeCalculator(_mockCurrencyConfig.Object);
            var price = 10.00m;
            var payment = new Payment
            {
                TotalPaid = 20.00m,
                Denominations = new Dictionary<decimal, int>
                {
                    {3.00m, 1} // Invalid denomination
                }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => calculator.CalculateChange(price, payment, "USD"));
            Assert.Contains("Invalid denomination: 3.00", exception.Message);
        }

        [Fact]
        public void CalculateChange_WithMissingCurrencyConfiguration_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _mockCurrencyConfig.Setup(config => config.GetCurrency())
                .Returns((CurrencyData) null); // Simulate missing configuration

            var calculator = new ChangeCalculator(_mockCurrencyConfig.Object);
            var price = 10.00m;
            var payment = new Payment
            {
                TotalPaid = 20.00m,
                Denominations = new Dictionary<decimal, int>
                {
                    {20.00m, 1}
                }
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => calculator.CalculateChange(price, payment, "USD"));
            Assert.Contains("The currency code 'USD' is not currently loaded.", exception.Message);
        }

        [Fact]
        public void CalculateChange_WhenExactChangeIsNotPossible_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _mockCurrencyConfig.Setup(config => config.GetCurrency())
                .Returns(new CurrencyData
                {
                    CurrencyCode = "USD",
                    Denominations = new List<decimal> {0.25m, 1.00m, 5.00m} // Reduced denominations for testing
                });

            var calculator = new ChangeCalculator(_mockCurrencyConfig.Object);
            var price = 14.30m; // Change of $0.70 cannot be returned exactly with available denominations
            var payment = new Payment
            {
                TotalPaid = 15.00m,
                Denominations = new Dictionary<decimal, int>
                {
                    {10.00m, 1},
                    {5.00m, 1}
                }
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => calculator.CalculateChange(price, payment, "USD"));
            Assert.Contains("Unable to provide exact change with available denominations.", exception.Message);
        }
    }
}