using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Queries.GetOrdersByUser
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Result<List<OrderDto>>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersByUserIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Result<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetOrdersByUserId(request.UserId.ToString());
        }
    }
}
