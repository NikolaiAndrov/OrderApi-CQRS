using FluentValidation;

namespace OrderApiCQRS.Application.Features.Orders.Commands.Validators
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Id).LessThanOrEqualTo(int.MaxValue);
            RuleFor(x => x.CustomerFirstName).NotEmpty().Length(2, 30);
            RuleFor(x => x.CustomerLastName).NotEmpty().Length(2, 30);
            RuleFor(x => x.Status).NotEmpty().Length(2, 15);
            RuleFor(x => x.TotalAmount).GreaterThan(0);
        }
    }
}
