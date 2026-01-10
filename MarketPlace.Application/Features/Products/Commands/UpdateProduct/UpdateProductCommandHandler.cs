using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null) return Result.Fail("El producto no existe");

            product.UpdatePrice(request.Price);
            product.UpdateStock(request.Stock);
            product.UpdateName(request.Name);
            product.UpdateDescription(request.Description);
            product.ChangeCategory(product.Category);
            if(product.State == ProductState.Active) product.Activate();
            else product.Deactivate();

            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
