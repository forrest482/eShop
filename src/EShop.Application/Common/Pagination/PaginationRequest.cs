namespace EShop.Application.Common.Pagination;
public record PaginationRequest(int PageIndex = 0, int PageSize = 10);
