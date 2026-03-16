using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Application.Dtos;
using MarketPlace.Domain;
using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = Category.Create(request.Name, request.Description);
            if (!result.Success)            
                return Result<CategoryDto>.Fail(result.Message);            

            await _categoryRepository.AddAsync(result.Data!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);                      

            return Result<CategoryDto>.Ok(_mapper.Map<CategoryDto>(result.Data));
        }
    }
}
