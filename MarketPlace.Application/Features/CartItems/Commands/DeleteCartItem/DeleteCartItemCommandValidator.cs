using FluentValidation;

namespace MarketPlace.Application.Features.CartItems.Commands.DeleteCartItem
{
    internal class DeleteCartItemCommandValidator : AbstractValidator<DeleteCartItemCommand>
    {
        public DeleteCartItemCommandValidator()
        {
            RuleFor(x => x.CartItemId)
                .NotEmpty().WithMessage("Cart item ID is required.")
                .Must(BeAValidGuid).WithMessage("Cart item ID must be a valid GUID.");
        }

        private static bool BeAValidGuid(string? value) => Guid.TryParse(value, out _);
    }
}
