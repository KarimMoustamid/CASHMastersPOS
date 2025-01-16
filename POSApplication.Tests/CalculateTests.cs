using System.Collections.Generic;
using POSApplication.BusinessLogic.Utilities.Payments;
using Xunit;

public class CalculateTests
{
    [Fact]
    public void TotalPaid_WithValidData_ReturnsCorrectTotal()
    {
        // Arrange
        var calculate = new Calculate();
        var paymentInDenominations = new Dictionary<decimal, int>
        {
            { 10m, 2 },   // Two 10-unit bills
            { 5m, 3 },    // Three 5-unit bills
            { 1m, 5 }     // Five 1-unit coins
        };

        // Act
        var result = calculate.TotalPaid(paymentInDenominations);

        // Assert
        Assert.Equal(40m, result); // 2*10 + 3*5 + 5*1 = 40
    }

    [Fact]
    public void TotalPaid_WithEmptyDictionary_ReturnsZero()
    {
        // Arrange
        var calculate = new Calculate();
        var paymentInDenominations = new Dictionary<decimal, int>();

        // Act
        var result = calculate.TotalPaid(paymentInDenominations);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void TotalPaid_WithZeroDenominations_ReturnsZero()
    {
        // Arrange
        var calculate = new Calculate();
        var paymentInDenominations = new Dictionary<decimal, int>
        {
            { 10m, 0 },
            { 5m, 0 }
        };

        // Act
        var result = calculate.TotalPaid(paymentInDenominations);

        // Assert
        Assert.Equal(0m, result); // No quantity => total is 0
    }

    [Fact]
    public void TotalPaid_WithNegativeQuantities_ReturnsCorrectTotal()
    {
        // Arrange
        var calculate = new Calculate();
        var paymentInDenominations = new Dictionary<decimal, int>
        {
            { 10m, -2 },
            { 5m, -3 }
        };

        // Act
        var result = calculate.TotalPaid(paymentInDenominations);

        // Assert
        Assert.Equal(-35m, result); // -2*10 + -3*5 = -35
    }
}