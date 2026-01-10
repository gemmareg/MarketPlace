using FluentValidation;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsListByCategory
{
    public class GetProductsListByCategoryQueryValidator : AbstractValidator<GetProductsListByCategoryQuery>
    {
        public GetProductsListByCategoryQueryValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId must not be empty.")
                .Must(BeAValidGuid).WithMessage("El valor no es un Guid válido");
        }

        private static bool BeAValidGuid(string? value) => Guid.TryParse(value, out _);
    }
}
