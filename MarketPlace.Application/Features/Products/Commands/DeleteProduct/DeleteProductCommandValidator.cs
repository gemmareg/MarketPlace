using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("Id must be a valid Guid");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("UserId must be a valid Guid");
        }
    }
}
