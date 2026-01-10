using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Result<List<ProductDto>>>
    {
        public string ProductId { get; set; }
    }
}
