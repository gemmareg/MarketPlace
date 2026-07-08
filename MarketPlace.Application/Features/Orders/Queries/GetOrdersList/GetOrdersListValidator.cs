using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListValidator : AbstractValidator<GetOrdersListQuery>
    {
        public GetOrdersListValidator()
        {
            RuleFor(x => x.CustomerId)
                .Must(ValidationHelpers.BeAValidGuid).When(x => x.CustomerId != null).WithMessage("CustomerId must be a valid GUID.");
            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate).When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("FromDate must be less than or equal to ToDate.");
        }
    }
}
