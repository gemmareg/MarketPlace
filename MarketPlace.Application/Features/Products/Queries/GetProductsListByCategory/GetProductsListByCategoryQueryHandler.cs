using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsListByCategory
{
    public class GetProductsListByCategoryQueryHandler : IRequestHandler<GetProductsListByCategoryQuery, Result<List<ProductDto>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetProductsListByCategoryQueryHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<ProductDto>>> Handle(GetProductsListByCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(new Guid(request.CategoryId!));
            if (category == null)
                return Result<List<ProductDto>>.Fail("Category not found.");

            var products = await _productRepository.GetProductsByCategoryIdAsync(category.Id);
            if (products == null || products.Count == 0)
                return Result<List<ProductDto>>.Fail("No products found in the specified category.");

            return Result<List<ProductDto>>.Ok(_mapper.Map<List<ProductDto>>(products));
        }
    }
}
