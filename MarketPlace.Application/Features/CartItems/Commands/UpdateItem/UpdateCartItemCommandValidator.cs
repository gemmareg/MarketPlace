using FluentValidation;

namespace MarketPlace.Application.Features.CartItems.Commands.UpdateItem
{
    public class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand>
    {
        public UpdateCartItemCommandValidator()
        {
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(x => x.CartItemId)
                .NotEmpty().WithMessage("CartItemId is required.")
                .Must(BeAValidGuid).WithMessage("CartItemId must be a valid GUID.");
        }

        private static bool BeAValidGuid(string? value) => Guid.TryParse(value, out _);
    }
}
