using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler(IProductService productService) : IRequestHandler<DeleteProductCommand, Result>
    {
        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await productService.DeleteProduct(request.Id, request.UserId, request.HasDeleteAnyPermission);
        }
    }
}
