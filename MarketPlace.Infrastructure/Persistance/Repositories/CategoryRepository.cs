using MarketPlace.Infrastructure.Persistance.Repositories.Common;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.Persistance.Context;
using MarketPlace.Application.Abstractions.Repositories;

namespace MarketPlace.Infrastructure.Persistance.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(MarketPlaceDbContext context) : base(context)
        {
        }
    }
}
