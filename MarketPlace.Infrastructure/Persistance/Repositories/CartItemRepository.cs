using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.Persistance.Context;
using MarketPlace.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Infrastructure.Persistance.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(MarketPlaceDbContext context) : base(context)
        {
        }

        public async Task<List<CartItem>> GetCartItemsByUserId(Guid userId) => await _context.Set<CartItem>().Where(ci => ci.UserId == userId).ToListAsync();
    }
}
