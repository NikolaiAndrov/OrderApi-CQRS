using MediatR;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Orders.Queries
{
    public record GetAllOrdersQuery(int page, int ordersCount) : IRequest<ICollection<ViewOrderDto>>;
}
