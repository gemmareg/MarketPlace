using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsListByCategory
{
    public class GetProductsListByCategoryQueryValidator : AbstractValidator<GetProductsListByCategoryQuery>
    {
        public GetProductsListByCategoryQueryValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId must not be empty.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("El valor no es un Guid válido");
        }
    }
}
