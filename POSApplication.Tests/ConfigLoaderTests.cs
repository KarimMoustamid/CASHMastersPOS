using System;
using Microsoft.Extensions.Configuration;
using POSApplication.BusinessLogic.config;
using Xunit;

public class ConfigLoaderTests
{
    [Fact]
    public void LoadConfiguration_ShouldNotReturnNull()
    {
        // Act
        var config = ConfigLoader.LoadConfiguration();

        // Assert
        Assert.NotNull(config);
    }

    [Fact]
    public void LoadConfiguration_ShouldContainExpectedKeyValues()
    {
        // Act
        var config = ConfigLoader.LoadConfiguration();

        // Example: Assuming appsettings.json contains "CurrencyFilePath"
        var currencyFilePath = config["CurrencyFilePath"];

        // Assert
        Assert.NotNull(currencyFilePath); // Make sure the key exists
        Assert.False(string.IsNullOrEmpty(currencyFilePath)); // Make sure it's not empty
    }

    [Fact]
    public void GetCurrencyFilePath_ShouldReturnExpectedValue()
    {
        // Arrange
        // Assuming appsettings.json contains CurrencyFilePath with some value

        // Act
        var currencyFilePath = ConfigLoader.GetCurrencyFilePath();

        // Assert
        Assert.NotNull(currencyFilePath);
        Assert.NotEmpty(currencyFilePath); // Check the value is not null or empty
    }

}