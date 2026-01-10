using MarketPlace.Application.Abstractions.Repositories.Common;
using MarketPlace.Domain;

namespace MarketPlace.Application.Abstractions.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByIdsAsync(List<Guid> Ids);
        Task<List<Product>> GetProductsByNameAsync(string name);
        Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId);
    }
}
