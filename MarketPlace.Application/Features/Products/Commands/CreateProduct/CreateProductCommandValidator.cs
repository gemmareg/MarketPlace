using FluentValidation;

namespace MarketPlace.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private const int MINIMAL_STOCK = 0;

        public CreateProductCommandValidator() 
        { 
            RuleFor(x => x.Name).NotEmpty().WithMessage("El título del producto no puede estar vacío.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("La descripción del producto no puede estar vacía.");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Debe tener una categoría asociada.");
            RuleFor(x => x.SellerId).NotEmpty().WithMessage("Debe tener un vendedor asociado.");
            RuleFor(x => x.Stock).GreaterThan(MINIMAL_STOCK).WithMessage("El stock debe ser mayor que 0");
            RuleFor(x => x.Price).Must(p => p >= 0).WithMessage("El precio no puede ser negativo");
        }
    }
}
