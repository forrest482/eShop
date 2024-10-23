namespace EShop.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductCommandRepository _repository;

    public CreateProductCommandHandler(IProductCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.Of(Guid.NewGuid());
        var title = ProductTitle.Of(request.Title);
        var price = Money.Of(request.Price, request.Currency);

        var product = Product.Create(productId, title, price);

        await _repository.AddAsync(product);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id.Value;
    }
}

