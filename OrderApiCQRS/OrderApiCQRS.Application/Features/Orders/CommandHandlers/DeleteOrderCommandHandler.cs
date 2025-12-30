using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Application.Exceptions;
using OrderApiCQRS.Application.Features.Orders.Commands;
using OrderApiCQRS.Data;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Application.Features.Orders.CommandHandlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly WriteDbContext dbContext;

        public DeleteOrderCommandHandler(WriteDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            Order? order = await this.dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null)
            {
                throw new NotFoundException($"Order with id {request.Id} was not found!");
            }

            this.dbContext.Orders.Remove(order);
            await this.dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
