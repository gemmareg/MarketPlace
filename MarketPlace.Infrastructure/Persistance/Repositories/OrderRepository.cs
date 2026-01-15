using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.Persistance.Context;
using MarketPlace.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MarketPlace.Infrastructure.Persistance.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(MarketPlaceDbContext context) : base(context)
        {
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _context.Orders.Where(o => o.User.Id == userId).ToListAsync();
            return orders;
        }
    }
}
