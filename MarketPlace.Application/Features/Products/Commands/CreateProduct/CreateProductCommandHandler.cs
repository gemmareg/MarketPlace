using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler(IProductService productService) : IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            return await productService.CreateProduct(request);
        }
    }
}
