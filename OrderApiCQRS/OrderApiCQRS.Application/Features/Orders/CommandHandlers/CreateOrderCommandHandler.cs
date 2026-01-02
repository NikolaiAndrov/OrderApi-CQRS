using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OrderApiCQRS.Application.Events.Orders;
using OrderApiCQRS.Application.Features.Products.Commands;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Application.Features.Products.CommandHandlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
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

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
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
            (
                order.Id,
                request.CustomerFirstName,
                request.CustomerLastName,
                request.Status,
                order.TotalAmount
            );

            await this.mediator.Publish(orderCreatedEvent, cancellationToken);

            return order.Id;
        }
    }
}
