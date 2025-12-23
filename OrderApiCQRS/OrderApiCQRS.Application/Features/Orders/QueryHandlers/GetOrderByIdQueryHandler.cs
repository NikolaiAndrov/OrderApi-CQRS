using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Features.Products.Queries;
using OrderApiCQRS.Data;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Products.QueryHandlers
{
    public class GetOrderByIdQueryHandler
    {
        public static async Task<ViewOrderDto?> Handle(GetOrderByIdQuery getOrderByIdQuery, AppDbContext dbContext)
        {
            ViewOrderDto? viewOrderDto = await dbContext.Orders
                .AsNoTracking()
                .Where(o => o.Id == getOrderByIdQuery.Id)
                .Select(o => new ViewOrderDto
                {
                    Id = o.Id,
                    CustomerFulltName = $"{o.CustomerFirstName} {o.CustomerLastName}",
                    Status = o.Status,
                    TotalAmount = o.TotalAmount
                })
                .FirstOrDefaultAsync();

            return viewOrderDto;
        }
    }
}
