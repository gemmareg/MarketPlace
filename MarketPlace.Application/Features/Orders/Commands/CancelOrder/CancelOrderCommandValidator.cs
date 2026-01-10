using FluentValidation;

namespace MarketPlace.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.")
                .Must(BeAValidGuid).WithMessage("UserId must be a valid GUID.");

        }
        private bool BeAValidGuid(string value) => Guid.TryParse(value, out _);
    }
}
