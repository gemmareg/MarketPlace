using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
