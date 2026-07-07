using MarketPlace.Application.Dtos;
using MarketPlace.Application.Features.Products.Commands.CreateProduct;
using MarketPlace.Application.Features.Products.Commands.UpdateProduct;
using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;

namespace MarketPlace.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<Result<List<ProductDto>>> getProductsList(string search);
        Task<Result<List<ProductDto>>> GetProductsListByCategory(string categoryId);
        Task<Result<List<ProductDto>>> GetProductsListBySeller(string sellerId);
        Task<Result<ProductDto>> GetProductById(string productId);
        Task<Result<Guid>> CreateProduct(CreateProductCommand request);
        Task<Result> DeleteProduct(string productId, string userId, bool hasDeleteAnyPermission);
        Task<Result> UpdateProduct(UpdateProductCommand request);
    }
}
