using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Features.Orders.Queries;
using OrderApiCQRS.Data;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Orders.QueryHandlers
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, ICollection<ViewOrderDto>>
    {
        private readonly ReadDbContext dbContext;

        public GetAllOrdersQueryHandler(ReadDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<ViewOrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            int ordersToSkip = (request.page - 1) * request.ordersCount;

            ICollection<ViewOrderDto> orders = await this.dbContext.Orders
                .AsNoTracking()
                .OrderBy(o => o.Id)
                .Skip(ordersToSkip)
                .Take(request.ordersCount)
                .Select(o => new ViewOrderDto
                {
                    Id = o.Id,
                    CustomerFulltName = $"{o.CustomerFirstName} {o.CustomerLastName}",
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                })
                .ToListAsync(cancellationToken);

            return orders;
        }
    }
}
