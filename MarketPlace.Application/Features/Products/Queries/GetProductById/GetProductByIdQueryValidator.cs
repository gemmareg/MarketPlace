using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("ProductId must be a valid GUID.");
        }
    }
}
