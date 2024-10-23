using FluentValidation;

namespace EShop.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderLines)
            .NotEmpty().WithMessage("Order must have at least one order line");

        RuleForEach(x => x.OrderLines).ChildRules(line =>
        {
            line.RuleFor(x => x.ProductId).NotNull();
            line.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}
