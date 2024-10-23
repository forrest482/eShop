using EShop.Domain.Enums;
using EShop.Domain.Events;
using EShop.Domain.Exceptions;
using EShop.Domain.Models;
using EShop.Domain.ValueObjects;
using FluentAssertions;

namespace EShop.Domain.UnitTests.Models;

public class OrderTests
{
    [Fact]
    public void Create_ShouldCreateOrderWithPendingStatus()
    {
        // Arrange
        var orderId = OrderId.Of(Guid.NewGuid());

        // Act
        var order = Order.Create(orderId);

        // Assert
        order.Status.Should().Be(OrderStatus.Pending);
        order.Id.Should().Be(orderId);
        order.DomainEvents.Should().ContainSingle(e => e is OrderCreatedEvent);
    }

    [Fact]
    public void AddOrderLine_ShouldAddLineAndRaiseDomainEvent()
    {
        // Arrange
        var order = Order.Create(OrderId.Of(Guid.NewGuid()));
        var productId = ProductId.Of(Guid.NewGuid());
        var unitPrice = Money.Of(100m, "USD");
        var quantity = 2;

        // Act
        order.AddOrderLine(productId, unitPrice, quantity);

        // Assert
        order.OrderLines.Should().HaveCount(1);
        order.OrderLines.Single().ProductId.Should().Be(productId);
        order.OrderLines.Single().UnitPrice.Should().Be(unitPrice);
        order.OrderLines.Single().Quantity.Should().Be(quantity);
        order.DomainEvents.Should().ContainSingle(e => e is OrderLineAddedEvent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddOrderLine_WithInvalidQuantity_ShouldThrowException(int invalidQuantity)
    {
        // Arrange
        var order = Order.Create(OrderId.Of(Guid.NewGuid()));
        var productId = ProductId.Of(Guid.NewGuid());
        var unitPrice = Money.Of(100m, "USD");

        // Act & Assert
        var act = () => order.AddOrderLine(productId, unitPrice, invalidQuantity);
        act.Should().Throw<DomainException>()
           .WithMessage("*Quantity must be greater than zero.*");
    }

    [Fact]
    public void ConfirmOrder_WhenPending_ShouldChangeStatusAndRaiseDomainEvent()
    {
        // Arrange
        var order = Order.Create(OrderId.Of(Guid.NewGuid()));

        // Act
        order.ConfirmOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Confirmed);
        order.DomainEvents.Should().ContainSingle(e => e is OrderConfirmedEvent);
    }

    [Theory]
    [InlineData(OrderStatus.Confirmed)]
    [InlineData(OrderStatus.Completed)]
    [InlineData(OrderStatus.Cancelled)]
    public void ConfirmOrder_WhenNotPending_ShouldThrowException(OrderStatus invalidStatus)
    {
        // Arrange
        var order = Order.Create(OrderId.Of(Guid.NewGuid()));
        typeof(Order).GetProperty(nameof(Order.Status))!
            .SetValue(order, invalidStatus);

        // Act & Assert
        var act = () => order.ConfirmOrder();
        act.Should().Throw<DomainException>()
           .WithMessage("*Can only confirm pending orders.*");
    }
}
