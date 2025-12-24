using FluentValidation;

namespace OrderApiCQRS.Application.Features.Orders.Queries.Validators
{
    public class GetAllOrdersQueryValidator : AbstractValidator<GetAllOrdersQuery>
    {
        public GetAllOrdersQueryValidator()
        {
            RuleFor(x => x.page).GreaterThan(0);
            RuleFor(x => x.page).LessThanOrEqualTo(99999);
            RuleFor(x => x.ordersCount).GreaterThan(0);
            RuleFor(x => x.ordersCount).LessThanOrEqualTo(100);
        }
    }
}
