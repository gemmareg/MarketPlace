using MarketPlace.Application.Abstractions.Repositories.Common;
using MarketPlace.Application.Dtos;
using MarketPlace.Domain;

namespace MarketPlace.Application.Abstractions.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderWithCartItemsAndProductsByIdAsync(Guid id);
        Task<List<OrderResumedDto>> GetOrdersByUserIdAsync(Guid userId);
        Task<List<OrderResumedDto>> GetOrdersList(Guid userId, DateTime? fromDate, DateTime? toDate);
        Task<Order?> GetOrderByIdWithOrderItemsAsync(Guid id);
    }
}
