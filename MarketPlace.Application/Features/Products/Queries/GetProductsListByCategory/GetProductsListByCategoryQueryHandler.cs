using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsListByCategory
{
    public class GetProductsListByCategoryQueryHandler(IProductService productService) : IRequestHandler<GetProductsListByCategoryQuery, Result<List<ProductDto>>>
    {
        public async Task<Result<List<ProductDto>>> Handle(GetProductsListByCategoryQuery request, CancellationToken cancellationToken)
        {
            return await productService.GetProductsListByCategory(request.CategoryId);
        }
    }
}
