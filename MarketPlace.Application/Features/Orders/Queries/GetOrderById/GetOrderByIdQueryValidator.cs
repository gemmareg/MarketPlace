using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("OrderId must be a valid GUID.");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("UserId must be a valid GUID.");
        }
    }
}
