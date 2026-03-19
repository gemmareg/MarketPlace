using AutoMapper;
using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Abstractions.UnitOfWork;
using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.Products.Commands.CreateProduct;
using MarketPlace.Application.Features.Products.Commands.UpdateProduct;
using MarketPlace.Domain;
using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;
using MediatR;
using static MarketPlace.Shared.Enums;

namespace MarketPlace.Application.Services
{
    public class ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IProductService
    {
        public async Task<Result<List<ProductDto>>> getProductsList(string search)
        {
            var products = await productRepository.GetProductsByNameAsync(search);

            if (products == null || products.Count == 0)
                return Result<List<ProductDto>>.Fail("No products found matching the search criteria.");
            var productsDto = mapper.Map<List<ProductDto>>(products);

            return Result<List<ProductDto>>.Ok(productsDto);
        }

        public async Task<Result<List<ProductDto>>> GetProductsListByCategory(string categoryId)
        {
            var category = await categoryRepository.GetByIdAsync(new Guid(categoryId!));
            if (category == null)
                return Result<List<ProductDto>>.Fail("Category not found.");

            var products = await productRepository.GetProductsByCategoryIdAsync(category.Id);
            if (products == null || products.Count == 0)
                return Result<List<ProductDto>>.Fail("No products found in the specified category.");

            return Result<List<ProductDto>>.Ok(mapper.Map<List<ProductDto>>(products));
        }

        public async Task<Result<List<ProductDto>>> GetProductsListBySeller(string sellerId)
        {
            var products = await productRepository.GetProductsBySellerAsync(new Guid(sellerId));
            if (products == null || products.Count == 0)
                return Result<List<ProductDto>>.Fail("No products found matching the search criteria.");
            var productsDto = mapper.Map<List<ProductDto>>(products);
            return Result<List<ProductDto>>.Ok(productsDto);
        }

        public async Task<Result<ProductDto>> GetProductById(string productId)
        {
            var product = await productRepository.GetByIdAsync(new Guid(productId!));
            if (product == null)
                return Result<ProductDto>.Fail("Product not found.");

            return Result<ProductDto>.Ok(mapper.Map<ProductDto>(product));
        }

        public async Task<Result<Guid>> CreateProduct(CreateProductCommand request)
        {
            if (!Guid.TryParse(request.SellerId, out var sellerId))
                return Result<Guid>.Fail("Usuario autenticado inválido.");

            var seller = await userRepository.GetByIdAsync(sellerId);
            if (seller == null) return Result<Guid>.Fail("El usuario no existe");

            if (!Guid.TryParse(request.CategoryId, out var categoryId))
                return Result<Guid>.Fail("Categoría inválido.");

            var category = await categoryRepository.GetByIdAsync(categoryId);
            if (category == null) return Result<Guid>.Fail("La categoría no existe");

            var product = Product.Create(
                category,
                seller,
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                DateTime.Now,
                ProductState.Active);

            if (!product.Success) return Result<Guid>.Fail(product.Message);

            await productRepository.AddAsync(product.Data!);
            await unitOfWork.SaveChangesAsync();

            return Result<Guid>.Ok(product.Data!.Id);
        }

        public async Task<Result> DeleteProduct(string productId, string userId, bool isAdmin)
        {
            var product = await productRepository.GetByIdAsync(Guid.Parse(productId));
            if (product == null) return Result.Fail("El producto no existe.");

            if (!product.ValidateSeller(Guid.Parse(userId)) || isAdmin)
                return Result.Fail("No tienes permiso para eliminar este producto.");

            await productRepository.RemoveAsync(product);
            await unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<Result> UpdateProduct(UpdateProductCommand request)
        {
            var product = await productRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (product == null) return Result.Fail("El producto no existe");

            if (!product.ValidateSeller(Guid.Parse(request.UserId)) || request.IsAdmin)
                return Result.Fail("No tienes permiso para actualizar este producto.");
    
            var category = await categoryRepository.GetByIdAsync(Guid.Parse(request.CategoryId));

            product.UpdatePrice(request.Price);
            product.UpdateStock(request.Stock);
            product.UpdateName(request.Name);
            product.UpdateDescription(request.Description);
            product.ChangeCategory(product.Category);
            if (product.State == ProductState.Active) product.Activate();
            else product.Deactivate();

            await productRepository.UpdateAsync(product);
            await unitOfWork.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
