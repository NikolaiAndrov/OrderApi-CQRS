using MediatR;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Products.Queries
{
    public record GetOrderByIdQuery(int Id) : IRequest<ViewOrderDto?>;
}
