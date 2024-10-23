using FluentValidation;

namespace EShop.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandValidator : AbstractValidator<ConfirmOrderCommand>
{
    public ConfirmOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotNull();
    }
}
