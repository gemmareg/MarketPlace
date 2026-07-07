using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.CartItems.Commands.DeleteCartItem
{
    internal class DeleteCartItemCommandValidator : AbstractValidator<DeleteCartItemCommand>
    {
        public DeleteCartItemCommandValidator()
        {
            RuleFor(x => x.CartItemId)
                .NotEmpty().WithMessage("Cart item ID is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("Cart item ID must be a valid GUID.");
        }
    }
}
