using MarketPlace.Application.Abstractions.Repositories.Common;
using MarketPlace.Domain;

namespace MarketPlace.Application.Abstractions.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
    }
}
