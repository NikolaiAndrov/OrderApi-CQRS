using OrderApiCQRS.Application.Events.Interfaces;
using OrderApiCQRS.Application.Events.Orders;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Application.Events.Projections
{
    public class OrderCreatedProjectionHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly ReadDbContext dbContext;

        public OrderCreatedProjectionHandler(ReadDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task HandleAsync(OrderCreatedEvent evt)
        {
            Order order = new Order
            {
                Id = evt.Id,
                CustomerFirstName = evt.CustomerFirstName,
                CustomerLastName = evt.CustomerLastName,
                Status = evt.Status,
                TotalAmount = evt.TotalAmount
            };

            await this.dbContext.Orders.AddAsync(order);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
