using FluentValidation;

namespace MarketPlace.Application.Features.Orders.Commands.Send
{
    public class DeliverOrderCommandValidator : AbstractValidator<DeliverOrderCommand>
    {
        public DeliverOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.")
                .Must(BeAValidGuid).WithMessage("OrderId must be a valid GUID.");
        }

        private bool BeAValidGuid(string value) => Guid.TryParse(value, out _);
    }
}
