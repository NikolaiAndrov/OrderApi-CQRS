using FluentValidation;
using FluentValidation.Results;
using OrderApiCQRS.Application.Features.Interfaces;
using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Products.CommandHandlers
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, ViewOrderDto>
    {
        private readonly AppDbContext dbContext;
        private readonly IValidator<CreateOrderCommand> validator;

        public CreateOrderCommandHandler(AppDbContext dbContext, IValidator<CreateOrderCommand> validator)
        {
            this.dbContext = dbContext;
            this.validator = validator;
        }

        public async Task<ViewOrderDto?> HandleAsync(CreateOrderCommand createOrderCommand)
        {
            ValidationResult validationResult = await this.validator.ValidateAsync(createOrderCommand);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Order order = new Order
            {
                CustomerFirstName = createOrderCommand.CustomerFirstName,
                CustomerLastName = createOrderCommand.CustomerLastName,
                Status = createOrderCommand.Status,
                TotalAmount = createOrderCommand.TotalAmount
            };

            await this.dbContext.Orders.AddAsync(order);
            await this.dbContext.SaveChangesAsync();

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
