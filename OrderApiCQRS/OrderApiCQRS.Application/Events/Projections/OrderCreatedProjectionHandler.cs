using MediatR;
using OrderApiCQRS.Application.Events.Orders;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Application.Events.Projections
{
    public class OrderCreatedProjectionHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly ReadDbContext dbContext;

        public OrderCreatedProjectionHandler(ReadDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            Order order = new Order
            {
                CustomerFirstName = notification.CustomerFirstName,
                CustomerLastName = notification.CustomerLastName,
                Status = notification.Status,
                TotalAmount = notification.TotalAmount
            };

            await this.dbContext.Orders.AddAsync(order, cancellationToken);
            await this.dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
