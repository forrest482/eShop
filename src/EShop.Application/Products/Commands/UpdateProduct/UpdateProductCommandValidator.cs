using FluentValidation;

namespace EShop.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotNull();

        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 100)
            .WithMessage("Title must be between 3 and 100 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero");
    }
}
