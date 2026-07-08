using FluentValidation;
using MarketPlace.Shared;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Application.Features.Orders.Commands.MarkOrderAsPaid
{
    public class MarkOrderAsPaidCommandValidator : AbstractValidator<MarkOrderAsPaidCommand>
    {
        public MarkOrderAsPaidCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("OrderId must be a valid GUID.");
            RuleFor(x => x.PaymentMethod)
                .Must(BeAValidPaymentMethod).WithMessage("PaymentMethod must be a valid value.")
                .Must(NotBeUndefined).WithMessage("PaymentMethod cannot be Undefined.");
        }
        private bool BeAValidPaymentMethod(int value) => Enum.IsDefined(typeof(PaymentMethod), value); 
        private bool NotBeUndefined(int value) => value != (int)PaymentMethod.Undefined;
    }
}
