using FluentValidation;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsBySeller
{
    public class GetProductsBySellerQueryValidator : AbstractValidator<GetProductsBySellerQuery>
    {
        public GetProductsBySellerQueryValidator() 
        {
            RuleFor(x => x.SellerId)
                .NotEmpty().WithMessage("SellerId is required.")
                .Must(BeAValidGuid).WithMessage("SellerId must be a valid GUID.");
        }

        private static bool BeAValidGuid(string? value) => Guid.TryParse(value, out _);
    }
}
