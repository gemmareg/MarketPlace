using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsBySeller
{
    public class GetProductsBySellerQueryValidator : AbstractValidator<GetProductsBySellerQuery>
    {
        public GetProductsBySellerQueryValidator() 
        {
            RuleFor(x => x.SellerId)
                .NotEmpty().WithMessage("SellerId is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("SellerId must be a valid GUID.");
        }
    }
}
