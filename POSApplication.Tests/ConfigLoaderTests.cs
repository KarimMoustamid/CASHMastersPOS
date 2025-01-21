using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using POSApplication.Data.Models;
using Moq;
using Xunit;

public class ConfigLoaderTests
{
    private const string TestCurrencyConfigFileName = "CurrencyConfig.json";

    [Fact]
    public void LoadConfiguration_ShouldReturnSingletonInstance()
    {
        // Arrange
        var config = ConfigLoader.LoadConfiguration();

        // Act
        var configInstance2 = ConfigLoader.LoadConfiguration();

        // Assert
        Assert.NotNull(config);
        Assert.Same(config, configInstance2); // Ensure it's a singleton
    }

    [Fact]
    public void LoadAllCurrenciesFromConfig_ShouldReturnCurrencies_WhenConfigIsValid()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"Currencies:0:Country", "United States"},
                {"Currencies:0:CurrencyCode", "USD"},
                {"Currencies:0:Denominations:0", "100"},
                {"Currencies:0:Denominations:1", "20"},
                {"Currencies:0:Denominations:2", "1"},
                {"Currencies:1:Country", "Canada"},
                {"Currencies:1:CurrencyCode", "CAD"},
                {"Currencies:1:Denominations:0", "50"},
                {"Currencies:1:Denominations:1", "10"}
            })
            .Build();

        // Simulate internal behavior
        var lockField = typeof(ConfigLoader).GetField("_lock", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        var configField =
            typeof(ConfigLoader).GetField("_configuration", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

        lock (lockField.GetValue(null))
        {
            configField.SetValue(null, configuration);
        }

        // Act
        var currencies = ConfigLoader.LoadAllCurrenciesFromConfig();

        // Assert
        Assert.NotNull(currencies);
        Assert.Equal(2, currencies.Count);

        // Validate the first currency
        var usd = currencies.Find(c => c.CurrencyCode == "USD");
        Assert.NotNull(usd);
        Assert.Equal("United States", usd.Country);
        Assert.Equal(new List<decimal> {100, 20, 1}, usd.Denominations);

        // Validate the second currency
        var cad = currencies.Find(c => c.CurrencyCode == "CAD");
        Assert.NotNull(cad);
        Assert.Equal("Canada", cad.Country);
        Assert.Equal(new List<decimal> {50, 10}, cad.Denominations);
    }

    [Fact]
    public void GetConfigurationFilePath_ShouldReturnCorrectFilePath()
    {
        // Arrange
        string expectedPath = Path.Combine(AppContext.BaseDirectory, TestCurrencyConfigFileName);

        // Act
        var filePath = ConfigLoader.GetConfigurationFilePath();

        // Assert
        Assert.NotNull(filePath);
        Assert.Equal(expectedPath, filePath);
    }

    [Fact]
    public void LoadAllCurrenciesFromConfig_ShouldLogErrorAndRethrow_WhenExceptionOccurs()
    {
        // Arrange
        var mockLogger = new Mock<TextWriter>();
        Console.SetError(mockLogger.Object);

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>()) // Invalid configuration
            .Build();

        var lockField = typeof(ConfigLoader).GetField("_lock", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        var configField =
            typeof(ConfigLoader).GetField("_configuration", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

        lock (lockField.GetValue(null))
        {
            configField.SetValue(null, configuration);
        }

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => ConfigLoader.LoadAllCurrenciesFromConfig());

        // Verify that the exception was logged
        mockLogger.Verify(writer => writer.WriteLine(It.Is<string>(msg => msg.Contains("Error loading currencies from configuration"))),
            Times.AtLeastOnce);

        Assert.Equal("Failed to load currencies from configuration.", exception.Message);
    }
}