using FluentValidation;

namespace MarketPlace.Application.Features.CartItems.Commands.CreateCartItem
{
    public class CreateCartItemCommandValidator : AbstractValidator<CreateCartItemCommand>
    {
        public CreateCartItemCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(BeAValidGuid).WithMessage("UserId must be a valid GUID.");
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.")
                .Must(BeAValidGuid).WithMessage("ProductId must be greater than zero.");
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }

        private static bool BeAValidGuid(string? value) => Guid.TryParse(value, out _);
    }
}
