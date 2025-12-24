using FluentValidation;
using OrderApiCQRS.Application.Features.Products.Commands;

namespace OrderApiCQRS.Application.Features.Orders.CommandHandlers.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CustomerFirstName).NotEmpty().Length(2, 30);
            RuleFor(x => x.CustomerLastName).NotEmpty().Length(2, 30);
            RuleFor(x => x.Status).NotEmpty().Length(2, 15);
            RuleFor(x => x.TotalAmount).GreaterThan(0);
        }
    }
}
