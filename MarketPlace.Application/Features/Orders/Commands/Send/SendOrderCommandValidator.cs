using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Orders.Commands.Send
{
    public class SendOrderCommandValidator : AbstractValidator<SendOrderCommand>
    {
        public SendOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("OrderId must be a valid GUID.");
        }
    }
}
