using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Features.Interfaces;
using OrderApiCQRS.Application.Features.Products.Queries;
using OrderApiCQRS.Data;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Products.QueryHandlers
{
    public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, ViewOrderDto>
    {
        private readonly AppDbContext dbContext;

        public GetOrderByIdQueryHandler(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ViewOrderDto?> HandleAsync(GetOrderByIdQuery getOrderByIdQuery)
        {
            ViewOrderDto? viewOrderDto = await this.dbContext.Orders
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
