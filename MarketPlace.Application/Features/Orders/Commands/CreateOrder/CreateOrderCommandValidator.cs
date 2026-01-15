using FluentValidation;

namespace MarketPlace.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(BeAValidGuid).WithMessage("UserId must be a valid GUID.");

            RuleFor(x => x.CartItemIds)
                .NotEmpty().WithMessage("CartItemIds cannot be empty.")
                .Must(list => list.Count > 0).WithMessage("CartItemIds must have at least one item.")
                .ForEach(item =>
                    item.Must(BeAValidGuid).WithMessage("Each CartItemId must be a valid GUID.")
                );
        }

        private bool BeAValidGuid(string value) => Guid.TryParse(value, out _);
    }
}
