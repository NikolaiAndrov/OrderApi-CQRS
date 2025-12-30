using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OrderApiCQRS.Application.Events.Interfaces;
using OrderApiCQRS.Application.Events.Orders;
using OrderApiCQRS.Application.Features.Interfaces;
using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;
using OrderApiCQRS.DtoModels.Order;

namespace OrderApiCQRS.Application.Features.Products.CommandHandlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ViewOrderDto>
    {
        private readonly WriteDbContext dbContext;
        private readonly IValidator<CreateOrderCommand> validator;
        private readonly IMediator mediator;

        public CreateOrderCommandHandler(WriteDbContext dbContext, 
            IValidator<CreateOrderCommand> validator,
            IMediator mediator)
        {
            this.dbContext = dbContext;
            this.validator = validator;
            this.mediator = mediator;
        }

        public async Task<ViewOrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await this.validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Order order = new Order
            {
                CustomerFirstName = request.CustomerFirstName,
                CustomerLastName = request.CustomerLastName,
                Status = request.Status,
                TotalAmount = request.TotalAmount
            };

            await this.dbContext.Orders.AddAsync(order, cancellationToken);
            await this.dbContext.SaveChangesAsync(cancellationToken);

            OrderCreatedEvent orderCreatedEvent = new OrderCreatedEvent
            {
                Id = order.Id,
                CustomerFirstName = request.CustomerFirstName,
                CustomerLastName = request.CustomerLastName,
                Status = request.Status,
                TotalAmount = order.TotalAmount
            };

            await this.mediator.Publish(orderCreatedEvent);

            ViewOrderDto viewOrderDto = new ViewOrderDto
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
