using MediatR;

namespace OrderApiCQRS.Application.Features.Orders.Commands
{
    public record DeleteOrderCommand(int Id) : IRequest;
}
