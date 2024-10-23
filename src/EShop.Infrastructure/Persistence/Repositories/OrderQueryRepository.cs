namespace EShop.Infrastructure.Persistence.Repositories;

public class OrderQueryRepository : IOrderQueryRepository
{
    private readonly EShopDbContext _context;

    public OrderQueryRepository(EShopDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto?> GetByIdAsync(OrderId id)
    {
        return await _context.Orders.AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new OrderDto
            {
                Id = o.Id.Value,
                Status = o.Status.ToString(),
                TotalAmount = o.TotalPrice.Amount,
                Currency = o.TotalPrice.Currency,
                CreatedAt = o.CreatedAt,
                CreatedBy = o.CreatedBy,
                LastModified = o.LastModified,
                LastModifiedBy = o.LastModifiedBy,
                OrderLines = o.OrderLines
                    .Join(
                        _context.Products,
                        ol => ol.ProductId,
                        p => p.Id,
                        (ol, p) => new OrderLineDto
                        {
                            Id = ol.Id.Value,
                            ProductId = ol.ProductId.Value,
                            ProductName = p.Title.Value,
                            UnitPrice = ol.UnitPrice.Amount,
                            Currency = ol.UnitPrice.Currency,
                            Quantity = ol.Quantity,
                            TotalPrice = ol.TotalPrice.Amount
                        })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ProductSalesByDateDto>> GetProductSalesLastWeekAsync(DateTime endDate)
    {
        var startDate = endDate.AddDays(-6);

        return await _context.Orders.AsNoTracking()
                        .Where(o => o.Status == OrderStatus.Completed
                                 && o.CreatedAt >= startDate
                                 && o.CreatedAt <= endDate)
                        .SelectMany(o => o.OrderLines
                            .Select(ol => new
                            {
                                OrderDate = o.CreatedAt!.Value.Date,
                                ol.ProductId,
                                ol.Quantity,
                                Amount = ol.UnitPrice.Amount * ol.Quantity,
                                ol.UnitPrice.Currency
                            }))
                        .Join(
                            _context.Products,
                            ol => ol.ProductId,
                            p => p.Id,
                            (ol, p) => new
                            {
                                ProductName = p.Title.Value,
                                ol.OrderDate,
                                ol.Quantity,
                                ol.Amount,
                                ol.Currency
                            })
                        .GroupBy(x => new { x.ProductName, x.OrderDate })
                        .Select(g => new
                        {
                            g.Key.ProductName,
                            Date = g.Key.OrderDate,
                            Quantity = g.Sum(x => x.Quantity),
                            Amount = g.Sum(x => x.Amount),
                            Currency = g.First().Currency
                        })
                        .GroupBy(x => x.ProductName)
                        .Select(g => new ProductSalesByDateDto
                        {
                            ProductName = g.Key,
                            Sales = g.Select(s => new DailySale
                            {
                                Date = s.Date,
                                Quantity = s.Quantity,
                                Amount = s.Amount,
                                Currency = s.Currency
                            }).ToList()
                        })
                        .ToListAsync();

        //return await _context.Orders
        //           .AsNoTracking()
        //           .Where(o => o.Status == OrderStatus.Completed
        //               && o.CreatedAt >= startDate
        //               && o.CreatedAt <= endDate)
        //           .SelectMany(o => o.OrderLines.Select(ol => new
        //           {
        //               ProductName = ol.Product.Title.Value,
        //               Date = o.CreatedAt!.Value.Date,
        //               ol.Quantity,
        //               Amount = ol.UnitPrice.Amount * ol.Quantity,
        //               Currency = ol.UnitPrice.Currency
        //           }))
        //           .GroupBy(x => new { x.ProductName, x.Date })
        //           .Select(g => new
        //           {
        //               g.Key.ProductName,
        //               g.Key.Date,
        //               Quantity = g.Sum(x => x.Quantity),
        //               Amount = g.Sum(x => x.Amount),
        //               Currency = g.First().Currency
        //           })
        //           .GroupBy(x => x.ProductName)
        //           .Select(g => new ProductSalesByDateDto
        //           {
        //               ProductName = g.Key,
        //               Sales = g.Select(s => new DailySale
        //               {
        //                   Date = s.Date,
        //                   Quantity = s.Quantity,
        //                   Amount = s.Amount,
        //                   Currency = s.Currency
        //               })
        //               .OrderBy(s => s.Date)
        //               .ToList()
        //           })
        //           .ToListAsync();

    }


    public async Task<List<ProductSalesCountDto>> GetProductSalesInPeriodAsync(
        DateTime startDate,
        DateTime endDate)
    {
        return await _context.Orders.AsNoTracking()
            .Where(o => o.Status == OrderStatus.Completed
                && o.CreatedAt >= startDate
                && o.CreatedAt <= endDate)
            .SelectMany(o => o.OrderLines
                .Join(
                    _context.Products,
                    ol => ol.ProductId,
                    p => p.Id,
                    (ol, p) => new
                    {
                        ProductName = p.Title.Value,
                        ol.Quantity
                    }))
            .GroupBy(x => x.ProductName)
            .Select(g => new ProductSalesCountDto
            {
                ProductName = g.Key,
                TotalCount = g.Sum(x => x.Quantity)
            })
            .ToListAsync();
    }

    public async Task<PaginatedResult<OrderDto>> GetOrdersAsync(PaginationRequest pagination)
    {
        var query = _context.Orders.AsNoTracking();
        var count = await query.CountAsync();

        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip(pagination.PageIndex * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(o => new OrderDto
            {
                Id = o.Id.Value,
                Status = o.Status.ToString(),
                TotalAmount = o.TotalPrice.Amount,
                Currency = o.TotalPrice.Currency,
                CreatedAt = o.CreatedAt,
                CreatedBy = o.CreatedBy,
                LastModified = o.LastModified,
                LastModifiedBy = o.LastModifiedBy,
                OrderLines = o.OrderLines
                    .Join(
                        _context.Products,
                        ol => ol.ProductId,
                        p => p.Id,
                        (ol, p) => new OrderLineDto
                        {
                            Id = ol.Id.Value,
                            ProductId = ol.ProductId.Value,
                            ProductName = p.Title.Value,
                            UnitPrice = ol.UnitPrice.Amount,
                            Currency = ol.UnitPrice.Currency,
                            Quantity = ol.Quantity,
                            TotalPrice = ol.TotalPrice.Amount
                        })
                    .ToList()
            })
            .ToListAsync();

        return new PaginatedResult<OrderDto>(
            pagination.PageIndex,
            pagination.PageSize,
            count,
            orders);
    }
}