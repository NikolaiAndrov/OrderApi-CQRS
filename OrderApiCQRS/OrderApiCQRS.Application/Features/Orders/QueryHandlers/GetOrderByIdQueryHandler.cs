using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Features.Products.Queries;
using OrderApiCQRS.Data;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Products.QueryHandlers
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ViewOrderDto?>
    {
        private readonly ReadDbContext dbContext;

        public GetOrderByIdQueryHandler(ReadDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ViewOrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            ViewOrderDto? viewOrderDto = await this.dbContext.Orders
                .AsNoTracking()
                .Where(o => o.Id == request.Id)
                .Select(o => new ViewOrderDto
                {
                    Id = o.Id,
                    CustomerFulltName = $"{o.CustomerFirstName} {o.CustomerLastName}",
                    Status = o.Status,
                    TotalAmount = o.TotalAmount
                })
                .FirstOrDefaultAsync(cancellationToken);

            return viewOrderDto;
        }
    }
}
