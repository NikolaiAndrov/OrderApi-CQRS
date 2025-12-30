using MediatR;

namespace OrderApiCQRS.Application.Events.Orders
{
    public record OrderDeletedEvent(int Id) : INotification;
}
