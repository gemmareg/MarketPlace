using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsBySeller
{
    public class GetProductsBySellerQuery : IRequest<Result<List<ProductDto>>>
    {
        public string SellerId { get; set; }
    }
}
