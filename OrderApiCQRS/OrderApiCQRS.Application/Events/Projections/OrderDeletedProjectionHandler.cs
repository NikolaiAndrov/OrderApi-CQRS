using MediatR;
using OrderApiCQRS.Application.Events.Orders;
using OrderApiCQRS.Application.Exceptions;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Application.Events.Projections
{
    public class OrderDeletedProjectionHandler : INotificationHandler<OrderDeletedEvent>
    {
        private readonly ReadDbContext dbContext;

        public OrderDeletedProjectionHandler(ReadDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
        {
            Order? order = await this.dbContext.Orders.FindAsync(notification.Id, cancellationToken);

            if (order == null)
            {
                throw new NotFoundException($"Order with id {notification.Id} was not found!");
            }

            this.dbContext.Orders.Remove(order);
            await this.dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
