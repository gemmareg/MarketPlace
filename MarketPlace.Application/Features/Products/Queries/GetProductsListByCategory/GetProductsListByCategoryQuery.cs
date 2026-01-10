using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsListByCategory
{
    public class GetProductsListByCategoryQuery : IRequest<Result<List<ProductDto>>>
    {
        public string? CategoryId { get; set; }
    }
}
