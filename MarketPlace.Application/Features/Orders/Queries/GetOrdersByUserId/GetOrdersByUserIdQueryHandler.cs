using MarketPlace.Application.Abstractions.Services;
using MarketPlace.Application.Dtos;
using MarketPlace.Shared.Result.Generic;
using MediatR;

namespace MarketPlace.Application.Features.Orders.Queries.GetOrdersByUser
{
    public class GetOrdersByUserIdQueryHandler(IOrderService orderService) : IRequestHandler<GetOrdersByUserIdQuery, Result<List<OrderResumedDto>>>
    {
        public async Task<Result<List<OrderResumedDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await orderService.GetOrdersByUserId(request.UserId.ToString());
        }
    }
}
