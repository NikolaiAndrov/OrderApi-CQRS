namespace OrderApiCQRS.Application.Features.Orders.Queries
{
    public record GetAllOrdersQuery(int page, int itemsCount);
}
