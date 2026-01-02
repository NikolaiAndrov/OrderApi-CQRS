using MediatR;

namespace OrderApiCQRS.Application.Events.Orders
{
    public record OrderCreatedEvent(int Id, string CustomerFirstName, string CustomerLastName, string Status, decimal TotalAmount) : INotification;
}
