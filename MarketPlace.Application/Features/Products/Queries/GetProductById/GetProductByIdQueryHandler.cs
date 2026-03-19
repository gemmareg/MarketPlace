using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler(IProductService productService) : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await productService.GetProductById(request.ProductId);
        }
    }
}
