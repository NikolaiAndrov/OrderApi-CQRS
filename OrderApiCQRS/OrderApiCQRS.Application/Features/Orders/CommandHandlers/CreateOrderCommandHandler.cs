using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Products.CommandHandlers
{
    public class CreateOrderCommandHandler
    {
        public static async Task<ViewOrderDto?> Handle(CreateOrderCommand createOrderCommand, AppDbContext dbContext)
        {
            Order order = new Order
            {
                CustomerFirstName = createOrderCommand.CustomerFirstName,
                CustomerLastName = createOrderCommand.CustomerLastName,
                Status = createOrderCommand.Status,
                TotalAmount = createOrderCommand.TotalAmount
            };

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            ViewOrderDto? viewOrderDto = new ViewOrderDto
            {
                Id = order.Id,
                CustomerFulltName = $"{order.CustomerFirstName} {order.CustomerLastName}",
                Status = order.Status,
                TotalAmount = order.TotalAmount
            };

            return viewOrderDto;
        }
    }
}
