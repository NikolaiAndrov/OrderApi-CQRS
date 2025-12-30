using FluentValidation;
using FluentValidation.Results;
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
        private readonly IValidator<GetAllOrdersQuery> validator;

        public GetAllOrdersQueryHandler(ReadDbContext dbContext, IValidator<GetAllOrdersQuery> validator)
        {
            this.dbContext = dbContext;
            this.validator = validator;
        }

        public async Task<ICollection<ViewOrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await this.validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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
