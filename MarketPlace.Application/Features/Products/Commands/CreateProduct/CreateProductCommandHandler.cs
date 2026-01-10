using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Domain;
using MarketPlace.Shared.Result.Generic;
using MediatR;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null) return Result<Guid>.Fail("La categoría no existe");

            var seller = await _userRepository.GetByIdAsync(request.SellerId);
            if (seller == null) return Result<Guid>.Fail("El usuario no existe");

            var product = Product.Create(
                category, 
                seller, 
                request.Name, 
                request.Description, 
                request.Price, 
                request.Stock, 
                DateTime.Now,
                ProductState.Active);

            if(!product.Success) return Result<Guid>.Fail(product.Message);

            await _productRepository.AddAsync(product.Data!);
            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Ok(product.Data!.Id);
        }
    }
}
