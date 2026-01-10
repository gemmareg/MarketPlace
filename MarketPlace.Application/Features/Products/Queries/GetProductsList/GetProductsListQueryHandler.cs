using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsList
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, Result<List<ProductDto>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsListQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<ProductDto>>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProductsByNameAsync(request.Search);

            if (products == null || products.Count == 0)
                return Result<List<ProductDto>>.Fail("No products found matching the search criteria.");

            return Result<List<ProductDto>>.Ok(_mapper.Map<List<ProductDto>>(products));
        }
    }
}
