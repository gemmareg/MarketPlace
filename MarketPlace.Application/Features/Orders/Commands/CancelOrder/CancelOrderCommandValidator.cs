using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("UserId must be a valid GUID.");

        }
    }
}
