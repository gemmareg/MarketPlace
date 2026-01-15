using FluentValidation;

namespace MarketPlace.Application.Features.CartItems.Queries.GetCartItems
{
    public class GetCartItemsQueryValidator : AbstractValidator<GetCartItemsQuery>
    {
        public GetCartItemsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .Must(BeAValidGuid).WithMessage("User ID must be a valid GUID.");
        }

        private static bool BeAValidGuid(string? value) => Guid.TryParse(value, out _);
    }
}
