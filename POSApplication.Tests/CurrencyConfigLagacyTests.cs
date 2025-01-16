using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using POSApplication.Data.Models;
using Xunit;

public class CurrencyConfigLagacyTests
{
    [Fact]
    public void Instance_IsSingleton_ReturnsSameInstance()
    {
        var instance1 = CurrencyConfigLagacy.Instance;
        var instance2 = CurrencyConfigLagacy.Instance;

        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void Initialize_ValidFilePath_LoadsCurrencies()
    {
        // Arrange
        string validFilePath = "valid_config.json";
        var currencyFile = new CurrencyFile
        {
            Currencies = new List<CurrencyData>
            {
                new CurrencyData {CurrencyCode = "USD", Denominations = new List<decimal> {1, 5, 10}}
            }
        };

        File.WriteAllText(validFilePath, JsonSerializer.Serialize(currencyFile));

        // Act
        CurrencyConfigLagacy.Instance.Initialize(validFilePath);

        // Assert
        var availableCurrencies = CurrencyConfigLagacy.Instance.GetAvailableCurrencies();
        Assert.Single(availableCurrencies);
        Assert.Equal("USD", availableCurrencies[0]?.CurrencyCode);

        // Cleanup
        if (File.Exists(validFilePath))
        {
            File.Delete(validFilePath);
        }
    }

    [Fact]
    public void LoadFromFile_FileDoesNotExist_ThrowsFileNotFoundException()
    {
        // Arrange
        string invalidFilePath = "nonexistent.json";

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() =>
            CurrencyConfigLagacy.Instance.LoadFromFile(invalidFilePath));
    }

    [Fact]
    public void LoadFromFile_InvalidCurrencyFile_ThrowsInvalidDataException()
    {
        // Arrange
        string invalidFilePath = "invalid_config.json";
        File.WriteAllText(invalidFilePath, JsonSerializer.Serialize(new CurrencyFile {Currencies = null}));

        // Act & Assert
        Assert.Throws<InvalidDataException>(() =>
            CurrencyConfigLagacy.Instance.LoadFromFile(invalidFilePath));

        // Cleanup
        if (File.Exists(invalidFilePath))
        {
            File.Delete(invalidFilePath);
        }
    }

    [Fact]
    public void SetCurrency_ValidCurrency_SetsCurrentCurrency()
    {
        // Arrange
        var currencyData = new List<CurrencyData>
        {
            new CurrencyData {CurrencyCode = "USD", Denominations = new List<decimal> {1, 5, 10}}
        };

        CurrencyConfigLagacy.Instance.LoadFromFile(CreateMockCurrencyFile(currencyData));

        // Act
        CurrencyConfigLagacy.Instance.SetCurrency("USD");

        // Assert
        var selectedCurrency = CurrencyConfigLagacy.Instance.GetCurrency();
        Assert.Equal("USD", selectedCurrency.CurrencyCode);
        Assert.Equal(new List<decimal> {10, 5, 1}, CurrencyConfigLagacy.Instance.GetDenominations());
    }

    [Fact]
    public void SetCurrency_NullOrEmptyCurrencyCode_ThrowsArgumentNullException()
    {
        // Arrange
        var currencyData = new List<CurrencyData>
        {
            new CurrencyData {CurrencyCode = "USD", Denominations = new List<decimal> {1, 5, 10}}
        };

        CurrencyConfigLagacy.Instance.LoadFromFile(CreateMockCurrencyFile(currencyData));

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            CurrencyConfigLagacy.Instance.SetCurrency(null));
    }

    [Fact]
    public void SetCurrency_InvalidCurrency_ThrowsKeyNotFoundException()
    {
        // Arrange
        var currencyData = new List<CurrencyData>
        {
            new CurrencyData {CurrencyCode = "USD", Denominations = new List<decimal> {1, 5, 10}}
        };

        CurrencyConfigLagacy.Instance.LoadFromFile(CreateMockCurrencyFile(currencyData));

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() =>
            CurrencyConfigLagacy.Instance.SetCurrency("EUR"));
    }

    [Fact]
    public void SetCurrency_NoDenominations_ThrowsInvalidOperationException()
    {
        // Arrange
        var currencyData = new List<CurrencyData>
        {
            new CurrencyData {CurrencyCode = "JPY", Denominations = new List<decimal>()}
        };

        CurrencyConfigLagacy.Instance.LoadFromFile(CreateMockCurrencyFile(currencyData));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            CurrencyConfigLagacy.Instance.SetCurrency("JPY"));
    }

    [Fact]
    public void GetCurrency_NoCurrencySet_ReturnsDefaultCurrencyData()
    {
        // Act
        var currentCurrency = CurrencyConfigLagacy.Instance.GetCurrency();

        // Assert
        Assert.NotNull(currentCurrency);
        Assert.Null(currentCurrency.CurrencyCode);
    }

    // Helper method to create a mock currency file and return its path
    private string CreateMockCurrencyFile(List<CurrencyData> currencies)
    {
        string filePath = $"mock_currency_{Guid.NewGuid()}.json";
        var currencyFile = new CurrencyFile {Currencies = currencies};
        File.WriteAllText(filePath, JsonSerializer.Serialize(currencyFile));
        return filePath;
    }
}