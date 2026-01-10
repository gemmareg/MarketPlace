using MarketPlace.Shared.Result.NonGeneric;
using MediatR;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public ProductState State { get; set; }

    }
}
