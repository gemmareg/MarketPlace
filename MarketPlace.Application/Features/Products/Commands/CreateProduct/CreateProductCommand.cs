using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public string SellerId { get; set; }
        public int Stock { get; set; }
    }
}
