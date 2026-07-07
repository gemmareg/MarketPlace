using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.CartItems.Commands.CreateCartItem
{
    public class CreateCartItemCommandValidator : AbstractValidator<CreateCartItemCommand>
    {
        public CreateCartItemCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("UserId must be a valid GUID.");
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("ProductId must be greater than zero.");
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
