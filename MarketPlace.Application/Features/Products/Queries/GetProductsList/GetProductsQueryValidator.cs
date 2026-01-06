using FluentValidation;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsList
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsListQuery>
    {
        public GetProductsQueryValidator() 
        { 
            RuleFor(x => x.Search)
                .NotEmpty().WithMessage("Search term must not be empty.")
                .MaximumLength(100).WithMessage("Search term must not exceed 100 characters.");
        }
    }
}
