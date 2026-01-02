using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OrderApiCQRS.Application.Exceptions;
using OrderApiCQRS.Application.Features.Orders.Commands;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Application.Features.Orders.CommandHandlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand ,int>
    {
        private readonly WriteDbContext dbContext;
        private readonly IValidator<UpdateOrderCommand> validator;
        private readonly IMediator mediator;

        public UpdateOrderCommandHandler(WriteDbContext dbContext, IValidator<UpdateOrderCommand> validator, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.validator = validator;
            this.mediator = mediator;
        }

        public async Task<int> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            Order? order = await this.dbContext.Orders
                .FindAsync(request.Id, cancellationToken);

            if (order == null)
            {
                throw new NotFoundException($"Order with id {request.Id} was not found!");
            }

            ValidationResult validationResult = await this.validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            order.CustomerFirstName = request.CustomerFirstName;
            order.CustomerLastName = request.CustomerLastName;
            order.Status = request.Status;
            order.TotalAmount = request.TotalAmount;

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
