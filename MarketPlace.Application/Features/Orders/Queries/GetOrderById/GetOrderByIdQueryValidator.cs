using FluentValidation;

namespace MarketPlace.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.")
                .Must(BeAValidGuid).WithMessage("OrderId must be a valid GUID.");
        }
        private bool BeAValidGuid(string value) => Guid.TryParse(value, out _);
    }
}
