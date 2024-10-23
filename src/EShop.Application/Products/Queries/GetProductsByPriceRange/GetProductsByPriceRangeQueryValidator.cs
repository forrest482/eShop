using FluentValidation;

namespace EShop.Application.Products.Queries.GetProductsByPriceRange;

public class GetProductsByPriceRangeQueryValidator
    : AbstractValidator<GetProductsByPriceRangeQuery>
{
    public GetProductsByPriceRangeQueryValidator()
    {
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum price must be greater than or equal to zero");

        RuleFor(x => x.MaxPrice)
            .GreaterThan(x => x.MinPrice)
            .WithMessage("Maximum price must be greater than minimum price");
    }
}
