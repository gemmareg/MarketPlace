using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;

namespace MarketPlace.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<Result<Guid>> CreateOrder(string userId, List<string> cartItemIds);
        Task<Result> CancelOrder(string userId, string orderId);
        Task<Result> MarkOrderAsPaid(string orderId, int paymentMethod);
        Task<Result> SendOrder(string orderId, string userId);
        Task<Result> DeliverOrder(string orderId);
        Task<Result<OrderDto>> GetOrderById(string sellerId, string orderId, bool hasReadAnyPermission);
        Task<Result<List<OrderResumedDto>>> GetOrdersByUserId(string userId);
        Task<Result<List<OrderResumedDto>>> GetAllOrdersAsync(string? customerId, DateTime? from, DateTime? to);
    }
}
