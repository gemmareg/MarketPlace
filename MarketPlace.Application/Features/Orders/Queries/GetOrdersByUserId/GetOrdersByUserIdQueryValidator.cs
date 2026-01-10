using FluentValidation;

namespace MarketPlace.Application.Features.Orders.Queries.GetOrdersByUser
{
    public class GetOrdersByUserIdQueryValidator : AbstractValidator<GetOrdersByUserIdQuery>
    {
        public GetOrdersByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId cannot be empty.")
                .Must(BeAValidGuid).WithMessage("UserId must be a valid GUID.");
        }

        private bool BeAValidGuid(string value) => Guid.TryParse(value, out _);
    }
}
