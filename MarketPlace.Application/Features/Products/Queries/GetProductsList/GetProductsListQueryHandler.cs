using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsList
{
    public class GetProductsListQueryHandler(IProductService productService) : IRequestHandler<GetProductsListQuery, Result<List<ProductDto>>>
    {
        public async Task<Result<List<ProductDto>>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            return await productService.getProductsList(request.Search);
        }
    }
}
