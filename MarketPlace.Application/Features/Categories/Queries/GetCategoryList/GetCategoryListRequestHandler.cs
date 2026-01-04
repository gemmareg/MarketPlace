using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Categories.Queries.GetCategoryList
{
    public class GetCategoryListRequestHandler : IRequestHandler<GetCategoryListRequest, Result<List<CategoryListDto>>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryListRequestHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<CategoryListDto>>> Handle(GetCategoryListRequest request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryListDto>>(categories);

            return Result<List<CategoryListDto>>.Ok(categoriesDto);
        }
    }
}
