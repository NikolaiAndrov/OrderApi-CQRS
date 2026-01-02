using MediatR;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Orders.Commands
{
    public record UpdateOrderCommand(int Id, string CustomerFirstName, string CustomerLastName, string Status, decimal TotalAmount) : IRequest<ViewOrderDto?>;
}
