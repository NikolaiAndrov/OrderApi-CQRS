using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Events.Orders;
using OrderApiCQRS.Application.Exceptions;
using OrderApiCQRS.Application.Features.Orders.Commands;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Application.Features.Orders.CommandHandlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly WriteDbContext dbContext;
        private readonly IMediator mediator;

        public DeleteOrderCommandHandler(WriteDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            Order? order = await this.dbContext.Orders
                .FindAsync(request.Id, cancellationToken);

            if (order == null)
            {
                throw new NotFoundException($"Order with id {request.Id} was not found!");
            }

            this.dbContext.Orders.Remove(order);
            await this.dbContext.SaveChangesAsync(cancellationToken);

            OrderDeletedEvent orderDeletedEvent = new OrderDeletedEvent(request.Id);
            await this.mediator.Publish(orderDeletedEvent);
        }
    }
}
