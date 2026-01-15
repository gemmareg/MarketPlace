using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Queries.GetProductsList
{
    public class GetProductsListQuery : IRequest<Result<List<ProductDto>>>
    {
        public string Search {  get; set; }
    }
}
