using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Features.Interfaces;
using OrderApiCQRS.Application.Features.Orders.Queries;
using OrderApiCQRS.Data;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Orders.QueryHandlers
{
    public class GetAllOrdersQueryHandler : IQueryHandler<GetAllOrdersQuery, ICollection<ViewOrderDto>>
    {
        private readonly AppDbContext dbContext;

        public GetAllOrdersQueryHandler(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<ViewOrderDto>?> HandleAsync(GetAllOrdersQuery query)
        {
            int ordersToSkip = (query.page - 1) * query.ordersCount;

            ICollection<ViewOrderDto> orders = await this.dbContext.Orders
                .AsNoTracking()
                .OrderBy(o => o.Id)
                .Skip(ordersToSkip)
                .Take(query.ordersCount)
                .Select(o => new ViewOrderDto
                {
                    Id = o.Id,
                    CustomerFulltName = $"{o.CustomerFirstName} {o.CustomerLastName}",
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                })
                .ToListAsync();

            return orders;
        }
    }
}
