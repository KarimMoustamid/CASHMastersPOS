using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using POSApplication.Data;
using POSApplication.Data.Models;
using Xunit;

public class CurrencyConfigTests
{
    private readonly Mock<ILogger<CurrencyConfig>> _mockLogger;
    private readonly CurrencyConfig _currencyConfig;

    public CurrencyConfigTests()
    {
        _mockLogger = new Mock<ILogger<CurrencyConfig>>();
        _currencyConfig = new CurrencyConfig(_mockLogger.Object);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenLoggerIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CurrencyConfig(null!));
    }

    [Fact]
    public void Initialize_ShouldThrowException_WhenCurrenciesIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _currencyConfig.Initialize(null!));
    }

    [Fact]
    public void Initialize_ShouldThrowException_WhenCurrenciesAreEmpty()
    {
        // Arrange
        var currencies = new List<CurrencyData>();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _currencyConfig.Initialize(currencies));
        Assert.Equal("Pre-configured currencies cannot be empty. (Parameter 'preConfiguredCurrencies')", exception.Message);
    }


    [Fact]
    public void SetCurrency_ShouldThrowException_WhenCurrencyCodeNotFound()
    {
        // Arrange
        var currencies = new List<CurrencyData>
        {
            new CurrencyData {CurrencyCode = "USD", Denominations = new List<decimal> {1, 5, 10}}
        };

        _currencyConfig.Initialize(currencies);

        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() => _currencyConfig.SetCurrency("EUR"));
        Assert.Equal("Currency code 'EUR' was not found.", exception.Message);
    }

    [Fact]
    public void SetCurrency_ShouldThrowException_WhenDenominationsAreInvalid()
    {
        // Arrange
        var currencies = new List<CurrencyData>
        {
            new CurrencyData {CurrencyCode = "USD", Denominations = new List<decimal>()}
        };

        _currencyConfig.Initialize(currencies);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _currencyConfig.SetCurrency("USD"));
        Assert.Equal("Currency code 'USD' does not have any valid denominations.", exception.Message);
    }

    [Fact]
    public void SetCurrency_ShouldSetCurrency_WhenValidCurrencyCodeIsProvided()
    {
        // Arrange
        var currencies = new List<CurrencyData>
        {
            new CurrencyData {CurrencyCode = "USD", Denominations = new List<decimal> {10, 5, 1}}
        };

        _currencyConfig.Initialize(currencies);

        // Act
        _currencyConfig.SetCurrency("USD");

        // Assert
        var currentCurrency = _currencyConfig.GetCurrency();
        Assert.NotNull(currentCurrency);
        Assert.Equal("USD", currentCurrency.CurrencyCode);
        Assert.Equal(new List<decimal> {10, 5, 1}, _currencyConfig.GetDenominations());
    }

    [Fact]
    public void GetCurrency_ShouldReturnNull_WhenNoCurrencyIsSet()
    {
        // Act
        var currentCurrency = _currencyConfig.GetCurrency();

        // Assert
        Assert.Null(currentCurrency);
    }

    [Fact]
    public void GetAvailableCurrencies_ShouldReturnInitializedCurrencies()
    {
        // Arrange
        var currencies = new List<CurrencyData>
        {
            new CurrencyData {CurrencyCode = "USD", Denominations = new List<decimal> {1, 5, 10}},
            new CurrencyData {CurrencyCode = "EUR", Denominations = new List<decimal> {0.5m, 1, 2}}
        };

        _currencyConfig.Initialize(currencies);

        // Act
        var availableCurrencies = _currencyConfig.GetAvailableCurrencies();

        // Assert
        Assert.Equal(2, availableCurrencies.Count);
        Assert.Contains(availableCurrencies, c => c.CurrencyCode == "USD");
        Assert.Contains(availableCurrencies, c => c.CurrencyCode == "EUR");
    }

    [Fact]
    public void GetDenominations_ShouldThrowException_WhenNoCurrencyIsSet()
    {
        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _currencyConfig.GetDenominations());
        Assert.Equal("No currency has been selected.", exception.Message);
    }
}