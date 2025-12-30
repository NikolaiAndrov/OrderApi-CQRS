using MediatR;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Products.Commands
{
    public record CreateOrderCommand(string CustomerFirstName, string CustomerLastName, string Status, decimal TotalAmount) : IRequest<ViewOrderDto>;
}
