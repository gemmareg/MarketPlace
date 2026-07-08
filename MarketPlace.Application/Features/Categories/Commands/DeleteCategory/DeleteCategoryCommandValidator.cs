using FluentValidation;
using MarketPlace.Shared;

namespace MarketPlace.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Category ID is required.")
                .Must(ValidationHelpers.BeAValidGuid).WithMessage("Id must be a valid Guid");
        }
    }
}
