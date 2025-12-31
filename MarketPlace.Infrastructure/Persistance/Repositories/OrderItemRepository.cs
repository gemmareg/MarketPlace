using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.Persistance.Context;
using MarketPlace.Infrastructure.Persistance.Repositories.Common;

namespace MarketPlace.Infrastructure.Persistance.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(MarketPlaceDbContext context) : base(context)
        {
        }
    }
}
