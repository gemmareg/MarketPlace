using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;

namespace MarketPlace.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler(IProductService productService) : IRequestHandler<UpdateProductCommand, Result>
    {
        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            return await productService.UpdateProduct(request);
        }
    }
}
