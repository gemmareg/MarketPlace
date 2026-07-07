using FluentValidation;
using MarketPlace.Shared;

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
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("CartItemId must be a valid GUID.");
        }
    }
}
