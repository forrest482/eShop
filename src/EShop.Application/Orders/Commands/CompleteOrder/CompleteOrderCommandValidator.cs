using FluentValidation;

namespace EShop.Application.Orders.Commands.CompleteOrder;

public class CompleteOrderCommandValidator : AbstractValidator<CompleteOrderCommand>
{
    public CompleteOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotNull();
    }
}
