using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Orders.Queries.GetOrdersByUser
{
    public class GetOrdersByUserIdQueryValidator : AbstractValidator<GetOrdersByUserIdQuery>
    {
        public GetOrdersByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId cannot be empty.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("UserId must be a valid GUID.");
        }
    }
}
