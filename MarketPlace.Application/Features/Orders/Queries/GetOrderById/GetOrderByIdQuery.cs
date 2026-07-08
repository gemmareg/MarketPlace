using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<Result<OrderDto>>
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public bool HasReadAnyPermission { get; set; } = false;
    }
}
