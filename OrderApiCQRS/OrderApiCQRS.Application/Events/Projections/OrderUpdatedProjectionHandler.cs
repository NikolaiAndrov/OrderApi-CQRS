using MediatR;
using OrderApiCQRS.Application.Events.Orders;
using OrderApiCQRS.Application.Exceptions;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Application.Events.Projections
{
    public class OrderUpdatedProjectionHandler : INotificationHandler<OrderUpdatedEvent>
    {
        private readonly ReadDbContext dbContext;

        public OrderUpdatedProjectionHandler(ReadDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            Order? order = await this.dbContext.Orders
                .FindAsync( notification.Id,cancellationToken);

            if (order == null)
            {
                throw new NotFoundException($"Order with id {notification.Id} was not found!");
            }

            order.CustomerFirstName = notification.CustomerFirstName;
            order.CustomerLastName = notification.CustomerLastName;
            order.Status = notification.Status;
            order.TotalAmount = notification.TotalAmount;

            await this.dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
