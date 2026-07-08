using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public bool HasDeleteAnyPermission { get; set; }
        public string Id { get; set; }
    }
}
