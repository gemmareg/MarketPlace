using FluentValidation;

namespace MarketPlace.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.")
                .Must(BeAValidGuid).WithMessage("ProductId must be a valid GUID.");
        }

        private static bool BeAValidGuid(string? value) => Guid.TryParse(value, out _);
    }
}
