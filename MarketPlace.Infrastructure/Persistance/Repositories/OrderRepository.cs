using MarketPlace.Application.Abstractions.Repositories;
using MarketPlace.Application.Dtos;
using MarketPlace.Domain;
using MarketPlace.Infrastructure.Persistance.Context;
using MarketPlace.Infrastructure.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Infrastructure.Persistance.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(MarketPlaceDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetOrderWithCartItemsAndProductsByIdAsync(Guid id) => await _context.Orders
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.Id == id);

        public async Task<List<OrderResumedDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _context.Orders.Where(o => o.User.Id == userId)
                .Select(o => new OrderResumedDto
                {
                    OrderId = o.Id,
                    ItemsCount = o.OrderItems.Count(),
                    Total = o.Total,
                    DateCreated = o.CreatedDate
                }).ToListAsync();
            return orders;
        }

        public async Task<List<OrderResumedDto>> GetOrdersList(Guid userId, DateTime? fromDate, DateTime? toDate)
        {
            IQueryable<Order> query = _context.Orders;
            if(userId != Guid.Empty)
                query = query.Where(o => o.UserId == userId);
            if (fromDate.HasValue)
                query = query.Where(o => o.CreatedDate >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(o => o.CreatedDate <= toDate.Value);
            return await query.Select(o => new OrderResumedDto
            {
                OrderId = o.Id,
                ItemsCount = o.OrderItems.Count(),
                Total = o.Total,
                DateCreated = o.CreatedDate
            }).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdWithOrderItemsAsync(Guid id) => await _context.Orders
            .Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
    }
}
