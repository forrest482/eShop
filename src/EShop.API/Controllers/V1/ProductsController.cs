using EShop.Application.Products.Commands.CreateProduct;
using EShop.Application.Products.Commands.UpdateProduct;
using EShop.Application.Products.Queries.GetProductById;
using EShop.Application.Products.Queries.GetProducts;
using EShop.Application.Products.Queries.GetProductsByPriceRange;

namespace EShop.Api.Controllers.V1;

[ApiVersion("1.0")]
public class ProductsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<ProductDto>>> GetProducts(
        [FromQuery] PaginationRequest pagination)
    {
        var query = new GetProductsQuery { Pagination = pagination };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await Mediator.Send(query);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("by-price-range")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByPriceRange(
        [FromQuery] decimal minPrice,
        [FromQuery] decimal maxPrice)
    {
        var query = new GetProductsByPriceRangeQuery
        {
            MinPrice = minPrice,
            MaxPrice = maxPrice
        };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductCommand command)
    {
        var productId = await Mediator.Send(command);
        var query = new GetProductByIdQuery(productId);
        var product = await Mediator.Send(query);
        return CreatedAtAction(nameof(GetProduct), new { id = productId }, product);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await Mediator.Send(command);
        return NoContent();
    }

}
