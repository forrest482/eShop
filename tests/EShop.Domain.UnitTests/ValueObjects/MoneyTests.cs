using EShop.Domain.Exceptions;
using EShop.Domain.ValueObjects;
using FluentAssertions;

namespace EShop.Domain.UnitTests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Of_WithValidAmount_ShouldCreateMoney()
    {
        // Arrange
        var amount = 100m;
        var currency = "EUR";

        // Act
        var money = Money.Of(amount, currency);

        // Assert
        money.Amount.Should().Be(amount);
        money.Currency.Should().Be(currency);
    }

    [Fact]
    public void Of_WithDefaultCurrency_ShouldCreateMoneyWithUSD()
    {
        // Arrange
        var amount = 100m;

        // Act
        var money = Money.Of(amount);

        // Assert
        money.Amount.Should().Be(amount);
        money.Currency.Should().Be("USD");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public void Of_WithNegativeAmount_ShouldThrowException(decimal amount)
    {
        // Act
        var action = () => Money.Of(amount);

        // Assert
        action.Should().Throw<DomainException>()
            .WithMessage("*Amount cannot be negative.*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Of_WithInvalidCurrency_ShouldThrowException(string currency)
    {
        // Act
        var action = () => Money.Of(100, currency);

        // Assert
        action.Should().Throw<DomainException>()
            .WithMessage("*Currency cannot be empty.*");
    }

    [Fact]
    public void Add_WithSameCurrency_ShouldAddAmounts()
    {
        // Arrange
        var money1 = Money.Of(100m, "USD");
        var money2 = Money.Of(200m, "USD");

        // Act
        var result = money1 + money2;

        // Assert
        result.Amount.Should().Be(300m);
        result.Currency.Should().Be("USD");
    }


    [Fact]
    public void Add_WithDifferentCurrencies_ShouldThrowException()
    {
        // Arrange
        var money1 = Money.Of(100m, "USD");
        var money2 = Money.Of(200m, "EUR");

        // Act
        var action = () => money1 + money2;

        // Assert
        action.Should().Throw<DomainException>()
            .WithMessage("*Cannot add money with different currencies.*");
    }

    [Theory]
    [InlineData(2, 200)]
    [InlineData(3, 300)]
    [InlineData(0, 0)]
    public void Multiply_WithInteger_ShouldMultiplyAmount(int multiplier, decimal expectedAmount)
    {
        // Arrange
        var money = Money.Of(100m, "USD");

        // Act
        var result = money * multiplier;

        // Assert
        result.Amount.Should().Be(expectedAmount);
        result.Currency.Should().Be("USD");
    }

   
}