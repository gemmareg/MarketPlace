using MarketPlace.Application.Abstractions.Repositories;
using MediatR;
using MarketPlace.Domain;
using MarketPlace.Shared.Result.NonGeneric;
using MarketPlace.Application.Abstractions.UnitOfWork;

namespace MarketPlace.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = Category.Create(request.Name, request.Description);
            if (!result.Success)            
                return Result.Fail(result.Message);            

            await _categoryRepository.AddAsync(result.Data!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
