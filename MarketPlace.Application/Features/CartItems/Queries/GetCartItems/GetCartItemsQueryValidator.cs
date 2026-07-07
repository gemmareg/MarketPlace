using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.CartItems.Queries.GetCartItems
{
    public class GetCartItemsQueryValidator : AbstractValidator<GetCartItemsQuery>
    {
        public GetCartItemsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("User ID must be a valid GUID.");
        }
    }
}
