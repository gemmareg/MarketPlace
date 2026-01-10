using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;

namespace MarketPlace.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<Result> CreateOrder(string userId, List<string> cartItemIds);
        Task<Result> CancelOrder(string orderId);
        Task<Result> MarkOrderAsPaid(string orderId, int paymentMethod);
        Task<Result> SendOrder(string orderId);
        Task<Result> DeliverOrder(string orderId);
        Task<Result<OrderDto>> GetOrderById(string orderId);
        Task<Result<List<OrderDto>>> GetOrdersByUserId(string userId);
    }
}
