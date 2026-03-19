using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsBySeller
{
    public class GetProductsBySellerQueryHandler(IProductService productService) : IRequestHandler<GetProductsBySellerQuery, Result<List<ProductDto>>>
    {
        public async Task<Result<List<ProductDto>>> Handle(GetProductsBySellerQuery request, CancellationToken cancellationToken)
        {
            return await productService.GetProductsListBySeller(request.SellerId);
        }
    }
}
