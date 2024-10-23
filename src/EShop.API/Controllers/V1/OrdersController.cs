using EShop.Application.Orders.Commands.CancelOrder;
using EShop.Application.Orders.Commands.CompleteOrder;
using EShop.Application.Orders.Commands.ConfirmOrder;
using EShop.Application.Orders.Commands.CreateOrder;
using EShop.Application.Orders.Queries.GetOrderById;
using EShop.Application.Orders.Queries.GetProductSalesCount;
using EShop.Application.Orders.Queries.GetProductSalesLastWeek;

namespace EShop.Api.Controllers.V1;

[ApiVersion("1.0")]
public class OrdersController : ApiControllerBase
{

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
    {
        var query = new GetOrderByIdQuery(id);
        var result = await Mediator.Send(query);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("sales/weekly")]
    [ProducesResponseType(typeof(List<ProductSalesByDateDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductSalesByDateDto>>> GetWeeklySales(
     [FromQuery] DateTime? endDate)
    {
        var query = new GetProductSalesLastWeekQuery(endDate ?? DateTime.Now);
        var result = await Mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("sales/by-period")]
    [ProducesResponseType(typeof(List<ProductSalesCountDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductSalesCountDto>>> GetSalesByPeriod(
         [FromQuery] DateTime startDate,
         [FromQuery] DateTime endDate)
    {
        var query = new GetProductSalesCountQuery
        {
            StartDate = startDate,
            EndDate = endDate
        };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderCommand command)
    {
        var orderId = await Mediator.Send(command);
        var query = new GetOrderByIdQuery(orderId);
        var order = await Mediator.Send(query);
        return CreatedAtAction(nameof(GetOrder), new { id = orderId }, order);
    }

    [HttpPut("{id}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmOrder(Guid id)
    {
        var command = new ConfirmOrderCommand(id);
        await Mediator.Send(command);
        return NoContent();
    }


    [HttpPut("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteOrder(Guid id)
    {
        var command = new CompleteOrderCommand(id);
        await Mediator.Send(command);
        return NoContent();
    }


    [HttpPut("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        var command = new CancelOrderCommand(id);
        await Mediator.Send(command);
        return NoContent();
    }
}