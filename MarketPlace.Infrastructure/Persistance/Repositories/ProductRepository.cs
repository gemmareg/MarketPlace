using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.Persistance.Context;
using MarketPlace.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Infrastructure.Persistance.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MarketPlaceDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(List<Guid> Ids)
        {
            return await _dbSet.Where(p => Ids.Contains(p.Id)).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByNameAsync(string name)
        {
            return await _dbSet.Where(p => p.Name.Contains(name)).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            return await _dbSet.Where(p => p.CategoryId.ToString().Equals(categoryId.ToString())).ToListAsync();
        }
    }
}
