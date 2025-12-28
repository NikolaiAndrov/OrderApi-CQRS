using FluentValidation;
using FluentValidation.Results;
using OrderApiCQRS.Application.Events.Interfaces;
using OrderApiCQRS.Application.Events.Orders;
using OrderApiCQRS.Application.Features.Interfaces;
using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Products.CommandHandlers
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, ViewOrderDto>
    {
        private readonly WriteDbContext dbContext;
        private readonly IValidator<CreateOrderCommand> validator;
        private readonly IEventPublisher eventPublisher;

        public CreateOrderCommandHandler(WriteDbContext dbContext, 
            IValidator<CreateOrderCommand> validator, 
            IEventPublisher eventPublisher)
        {
            this.dbContext = dbContext;
            this.validator = validator;
            this.eventPublisher = eventPublisher;
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

            OrderCreatedEvent orderCreatedEvent = new OrderCreatedEvent
            {
                Id = order.Id,
                CustomerFirstName = createOrderCommand.CustomerFirstName,
                CustomerLastName = createOrderCommand.CustomerLastName,
                Status = createOrderCommand.Status,
                TotalAmount = order.TotalAmount
            };

            await this.eventPublisher.PublishAsyunc(orderCreatedEvent);

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
