namespace EShop.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductCommandRepository _repository;

    public UpdateProductCommandHandler(IProductCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(ProductId.Of(request.Id));
        if (product == null)
            throw new NotFoundException($"Product with ID {request.Id} not found");

        var title = ProductTitle.Of(request.Title);
        var price = Money.Of(request.Price, request.Currency);

        product.Update(title, price);

        await _repository.UpdateAsync(product);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
