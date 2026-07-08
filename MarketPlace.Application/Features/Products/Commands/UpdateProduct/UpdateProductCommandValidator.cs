using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private const int MINIMAL_STOCK = 0;
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El título del producto no puede estar vacío.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("La descripción del producto no puede estar vacía.");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Debe tener una categoría asociada.");
            RuleFor(x => x.Stock).GreaterThan(MINIMAL_STOCK).WithMessage("El stock debe ser mayor que 0");
            RuleFor(x => x.Price).Must(p => p >= 0).WithMessage("El precio no puede ser negativo");

            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("Id must be a valid Guid");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("UserId must be a valid Guid");
        }
    }
}
